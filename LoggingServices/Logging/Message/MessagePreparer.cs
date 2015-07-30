using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Logging.Services
{
	public class MessagePreparer : IMessagePreparer, IDisposable
	{
		private BlockingCollection<ILogMessage> messageCollection; //can't be readonly because of issues clearing a collection. Basically not suported by BlockingCollection<T>
		private object syncObj = new object(); //we need to sync potential non-supported changes to the collection
		private  CancellationTokenSource currentTakeCancelToken = null;

		private readonly IThreadedAccessContainer<StringBuilder> stringBuilderService;

		public MessagePreparer(IThreadedAccessContainer<StringBuilder> sbService)
		{
			stringBuilderService = sbService;
			messageCollection = new BlockingCollection<ILogMessage>(); //TODO: Explore memory usage without upperbound for message queue.
		}

		public string Take()
		{
			ILogMessage message = null;

			while(message == null)
			{
				BlockingCollection<ILogMessage> localMessageQueueReference = null;

				lock (syncObj)
				{
					//Only make a new one if cancel is requested
					if (currentTakeCancelToken == null || currentTakeCancelToken.IsCancellationRequested)
						currentTakeCancelToken = new CancellationTokenSource(); //create a new cancelation token for taking.

					localMessageQueueReference = messageCollection;
				}

				//TODO: Rewrite this hack
				try
				{
					message = messageCollection.Take(currentTakeCancelToken.Token); //This could loop forever but if we can't take any non-null items there is a design issue.
				}
				catch(ObjectDisposedException e)
				{
					//This is bad practice to control flow by exceptions. However, if this does occur it means Clear was called while this was blocking and the cancelation didn't happen
					//quick enough. We just need to continue and things should be fine.
					continue;
				}
				catch(OperationCanceledException e)
				{
					//This means that during take it was canceled. We should just continue after this occurs.
					continue;
				}
			}

			//TODO: This is not working. Fix it so we can provide a StringBuilder to the message.
			if (stringBuilderService.isAvailable)
				try
				{
					using (var sb = stringBuilderService.Get())
					{
						//return message.BuildMessage(new StringBuilder());
						return message.BuildMessage(sb.Get());
					}
				}
				catch(Exception e)
				{
					return "Error: StringBuilder service unavailable or unable to build a " + typeof(ILogMessage).ToString() + " for other reasons. Exception: " + e.GetType().ToString() + " Message: " + e.Message
						+ " StackTrace: " + e.StackTrace;
				}
			else
				throw new Exception("Error: Message preparing cannot aquire a StringBuilder service.");
		}

		public void Add(ILogMessage item)
		{
			lock(syncObj) //We can just lock because we don't lock anymore while taking. Deadlocks used to occur before, and the fix didn't work 100%, so we now don't lock on Take().
							//We were able to do it lockless because we caught dipose and cancel exceptions and continued on them. There is little to no contention on syncObj anymore.
			{
				messageCollection.Add(item);
			}
		}

		public void Clear()
		{
			lock(syncObj)
			{
				if (messageCollection != null)
					messageCollection.Dispose();

				messageCollection = new BlockingCollection<ILogMessage>();
			}
		}

		public bool Contains(ILogMessage item)
		{
			lock (syncObj)
			{
				return messageCollection.Contains(item);
			}
		}

		public void CopyTo(ILogMessage[] array, int arrayIndex)
		{
			lock (syncObj)
			{
				try
				{
					messageCollection.CopyTo(array, arrayIndex);
				}
				catch(ObjectDisposedException e)
				{
					//This means somehow another thread disposed of the collection before a call to CopyTo
					//We do nothing right now though. We rethrow with more information.
					throw new Exception("Exception of type: " + typeof(ObjectDisposedException).ToString() + " occured in " + "CopyTo() method in " + this.GetType().ToString() + ". Object was already disposed.");
				}
			}
		}

		public int Count
		{
			get 
			{
				lock (syncObj)
				{
					return messageCollection.Count;
				}
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(ILogMessage item) //This method is highly expensive and shouldn't be called often.
		{
			lock(syncObj)
			{
				IEnumerable<ILogMessage> temporaryHolderOfMessages = messageCollection.Where(x => x != item).ToArray(); //gather every item that isn't this item. We use ToArray to generate a new collection. This must be done due to disposal

				if (temporaryHolderOfMessages.Count() == messageCollection.Count) //If these values are the same it means that the item requested wasn't found.
					return false;

				this.Clear(); //We have to clear to get a new collection.

				foreach(ILogMessage m in temporaryHolderOfMessages)
				{
					messageCollection.Add(m);
				}

				return true;
			}
		}

		public IEnumerator<ILogMessage> GetEnumerator()
		{
			lock (syncObj)
			{
				return ((IEnumerable<ILogMessage>)messageCollection).GetEnumerator();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			lock (syncObj)
			{
				//Call the generic version in this class.
				return this.GetEnumerator();
			}
		}

		public void Dispose()
		{
			lock(syncObj)
			{
				messageCollection.Dispose();
			}
		}
	}
}
