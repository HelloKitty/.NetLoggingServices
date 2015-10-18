using Logging.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace LoggingServices.Tests
{
	[TestFixture]
	public static class AsyncTextWriterLoggerTest
	{
		[Test]
		public static void LogMultipleMessageInParalell()
		{
			AsyncTextWriterLogger logger = new AsyncTextWriterLogger(LogLevel.All, new MessagePreparer(new StringBuilderContainer()), Console.Out);

			Parallel.For(0, 1000, (i) =>
				{
					logger.Log("Test {0} {1}", LogLevel.Debug, typeof(Thread), typeof(int), i);
					logger.Log("Test {0}", LogLevel.Debug, typeof(Thread), typeof(int));
					logger.Log("Test {0} {1}", LogLevel.Debug, "Testing No Caller", i);
					logger.Log("Test", LogLevel.Debug, typeof(Thread));

					logger.Log((object)"Test {0} {1}", LogLevel.Debug, typeof(Thread), typeof(int), i);
					logger.Log((object)"Test {0}", LogLevel.Debug, typeof(Thread), typeof(int));
					logger.Log((object)"Test {0} {1}", LogLevel.Debug, "Testing No Caller", i);
					logger.Log((object)"Test", LogLevel.Debug, typeof(Thread));

					foreach(LogLevel l in Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>())
					{
						logger.Log("Testing: " + l.ToString(), l);
						logger.Log((object)("Testing: " + l.ToString()), l);
					}
				});

			for(int i = 0; i < 1000; i++)
			{
				logger.Log("Test2 {0} {1}", LogLevel.Debug, typeof(Thread), typeof(int), i);
			}

			Task.Factory.StartNew(() =>
				{
					logger.Log("Test {0} {1}", LogLevel.Debug, typeof(Thread), typeof(int), 69);
				});

			Thread.Sleep(1000);

			logger.Dispose();
		}

		[Test]
		public static void TestLoggerState()
		{
			AsyncTextWriterLogger logger = new AsyncTextWriterLogger(LogLevel.All, new MessagePreparer(new StringBuilderContainer()), Console.Out);

			if (logger.State != LogLevel.All)
				throw new Exception("Logger.State is not being set properly.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public static void TrySupplyInvalidPreparer()
		{
			var logger = new AsyncTextWriterLogger(LogLevel.All, null, Console.Out);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public static void TrySupplyInvalidTextWriter()
		{
			var logger = new AsyncTextWriterLogger(LogLevel.All, new MessagePreparer(new StringBuilderContainer()), null);
		}
	}
}
