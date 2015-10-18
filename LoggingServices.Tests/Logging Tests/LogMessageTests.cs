using Logging.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LoggingServices.Tests
{
	[TestFixture]
	public static class LogMessageTests
	{
		[Test]
		public static void TestConstruction()
		{
			LogMessage message = new LogMessage("Hi: {0}", LogLevel.Error, typeof(LogMessageTests), 2);

			Assert.AreEqual(message.CallingType, typeof(LogMessageTests));
			Assert.AreEqual(message.Level, LogLevel.Error);
			Assert.IsNotNull(message.MainMessageObject);
			Assert.IsTrue(message.ObjParams != null && message.ObjParams.Count() == 1);
		}

		[Test]
		public static void TestBuildMessageWithParams()
		{
			LogMessage message = new LogMessage("Hi: {0}", LogLevel.Error, typeof(LogMessageTests), 2);

			Console.WriteLine(message.BuildMessage());
			Assert.IsTrue(message.BuildMessage().Contains("Hi: 2"));
			Assert.IsTrue(message.BuildMessage().Contains(typeof(LogMessageTests).ToString()));
			Assert.IsTrue(message.BuildMessage().Contains(message.Level.ToString()));
		}

		[Test]
		[ExpectedException]
		public static void TestBuildMessageWithoutParamsButExpectingParams()
		{
			LogMessage message = new LogMessage("Hi: {0}", LogLevel.Error, typeof(LogMessageTests));

			Console.WriteLine(message.BuildMessage());
		}

		[Test]
		public static void TestBuildMessageNoParams()
		{
			LogMessage message = new LogMessage("Hi: Blah", LogLevel.Error, typeof(LogMessageTests));

			Console.WriteLine(message.BuildMessage());
			Assert.IsTrue(message.BuildMessage().Contains("Hi: Blah"));
			Assert.IsTrue(message.BuildMessage().Contains(typeof(LogMessageTests).ToString()));
			Assert.IsTrue(message.BuildMessage().Contains(message.Level.ToString()));
		}

		[ExpectedException]
		[Test]
		public static void TestInvalidSizeOfObjParams()
		{
			LogMessage message = new LogMessage("Hi: {0} {1}", LogLevel.Error, typeof(LogMessageTests), 2);

			Console.WriteLine(message.BuildMessage());
		}

		[Test]
		public static void TestExtraObjParams()
		{
			LogMessage message = new LogMessage("Hi: {0} ", LogLevel.Error, typeof(LogMessageTests), 2, 3, 4, "TestingBlah");

			Console.WriteLine(message.BuildMessage());
			Assert.IsTrue(message.BuildMessage().Contains("Hi: 2"));
			Assert.IsFalse(message.BuildMessage().Contains("TestingBlah"));
		}
	}
}
