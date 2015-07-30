using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Services
{
	public class AsyncTextWriterLogger : ThreadedLogger, ILogger, IDisposable
	{
        public LogLevel State
        {
            get { return loggerLogLevel; }
        }

		private readonly IMessagePreparer messagePreparer;
		private readonly TextWriter writer;

		public AsyncTextWriterLogger(LogLevel level, IMessagePreparer preparer, TextWriter writerToWriteTo)
			: base(level)
		{
			if (writerToWriteTo == null)
				throw new ArgumentNullException("writerToWriteTo", "This logger requires a valid TextWriter, non-null, to write to.");

			if (preparer == null)
				throw new ArgumentNullException("preparer", "This logger requires a valid " + typeof(IMessagePreparer).ToString() + " to prepare messages with.");

			writer = writerToWriteTo;
			messagePreparer = preparer;
		}

		protected override bool TryConsumeMessage()
		{
			string message = messagePreparer.Take(); //blocks

			if (message != null)
				LogString(message);

			return message != null;
		}

		#region String Based Logging
		public void Log(string log, LogLevel level)
		{
			Log(log, level, null, null);
		}

		public void Log(string log, LogLevel level, Type caller)
		{
			Log(log, level, caller, null);
		}

		public void Log(string log, LogLevel level, params object[] objs)
		{
			Log(log, level, null, objs);
		}

		public void Log(string log, LogLevel level, Type caller, params object[] objs)
		{			
			if (loggerLogLevel.HasFlag(level)) //If we're not including that LogLevel then we don't queue up this log request.
#if DEBUG || DEBUGBUILD
			/*{
				messagePreparer.Add(new LogMessage(log, level, caller, objs)); //This can block on adding depending on collection
				Console.WriteLine("Added to messagePrep.");
			}
			else
				Console.WriteLine("Don't have log flag available: " + level.ToString());*/
			messagePreparer.Add(new LogMessage(log, level, caller, objs)); //This can block on adding depending on collection
#else
		messagePreparer.Add(new LogMessage(log, level, caller, objs)); //This can block on adding depending on collection
#endif
		}
		#endregion

		#region Object Based Logging
		public void Log(object obj, LogLevel level)
		{
			Log(obj, level, null, null);
		}

		public void Log(object obj, LogLevel level, Type caller)
		{
			Log(obj, level, caller, null);
		}

		public void Log(object obj, LogLevel level, params object[] objs)
		{
			Log(obj, level, null, objs);
		}

		public void Log(object obj, LogLevel level, Type caller, params object[] objs)
		{
			if (loggerLogLevel.HasFlag(level)) //If we're not including that LogLevel then we don't queue up this log request.
#if DEBUG || DEBUGBUILD
			/*{
				messagePreparer.Add(new LogMessage(obj, level, caller, objs)); //This can block on adding depending on collection
				Console.WriteLine("Added to messagePrep.");
			}
			else
				Console.WriteLine("Don't have log flag available: " + level.ToString());*/
		messagePreparer.Add(new LogMessage(obj, level, caller, objs)); //This can block on adding depending on collection
#else
		messagePreparer.Add(new LogMessage(obj, level, caller, objs)); //This can block on adding depending on collection
#endif
		}
		#endregion

		private void LogString(string message)
		{
			//TODO: Test preformance vs non-async
			writer.WriteLine(message);
		}

		public override void Dispose()
		{
			base.Dispose();

			messagePreparer.Dispose();
		}
    }
}
