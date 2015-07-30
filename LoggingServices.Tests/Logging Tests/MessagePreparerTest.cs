using Logging.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingServices.Test
{
	[TestFixture]
	public static class MessagePreparerTest
	{
		[Test]
		public static void MessagePreparer_ICollection_Functionality_Test()
		{
			IMessagePreparer preparer = new MessagePreparer(new StringBuilderContainer());

			if (!TestContainerSize(0, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			ILogMessage message = new LogMessage("Test", LogLevel.Debug, typeof(MessagePreparerTest)); 

			preparer.Add(message);

			if (!preparer.Contains(message))
				throw new Exception("Failed to find a message that was added in the collection via contains.");

			if (!TestContainerSize(1, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Remove(message);

			if (!TestContainerSize(0, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Add(message);

			if (!TestContainerSize(1, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			if(preparer.Remove(null))
			{
				throw new Exception("Preparer said it removed a null.");
			}

			preparer.Remove(new LogMessage("Test", LogLevel.Debug, typeof(MessagePreparerTest)));

			if (!TestContainerSize(1, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Add(new LogMessage("Test", LogLevel.Debug, typeof(MessagePreparerTest)));

			int count = 0;

			foreach(var l in preparer)
			{
				if (l == null)
					throw new Exception("Preparer's enumerator didn't work.");

				count++;
			}

			if (count != 2)
				throw new Exception("Didn't iterate over the whole collection.");

			if (!TestContainerSize(2, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Remove(message);

			if (!TestContainerSize(1, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Remove(message);

			if (!TestContainerSize(1, preparer))
				throw new Exception("MessagePreparer count is invalid.");

			preparer.Clear();

			if (!TestContainerSize(0, preparer))
				throw new Exception("MessagePreparer count is invalid.");
		}

		private static bool TestContainerSize(int expectedSize, ICollection<ILogMessage> collection)
		{
			return collection.Count == expectedSize;
		}

	}
}
