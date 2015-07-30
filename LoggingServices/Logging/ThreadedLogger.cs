using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Services
{
	public abstract class ThreadedLogger : IDisposable
	{
		private readonly Task consumerTask;
		protected readonly LogLevel loggerLogLevel;

		private bool _isPolling;
		protected bool isPolling
		{
			get { return _isPolling; }
			private set { _isPolling = value; }
		}

		public ThreadedLogger(LogLevel level)
		{
			loggerLogLevel = level;
			isPolling = true;
			consumerTask = Task.Factory.StartNew(MessageConsumerLoop, TaskCreationOptions.LongRunning); //Creates a long running task to consumer the messages supplied to the logger. Not promised to be a new thread but it is suppose to be.
		}

		private void MessageConsumerLoop()
		{
			while(isPolling)
			{
				TryConsumeMessage(); // We don't need to check if this is returning true or false.
			}
		}

		protected abstract bool TryConsumeMessage();

		public virtual void Dispose()
		{
			isPolling = false;
			//Cannot dispose task. No way to coordinate it. Let GC handle it.
		}
	}
}
