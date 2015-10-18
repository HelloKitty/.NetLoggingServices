using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logging.Services
{
	public class StreamLogger : ILogger, IDisposable
	{
        public LogLevel State { get; private set; }

		private readonly IMessagePreparer messagePreparer;
		private readonly TextWriter writer;

		public StreamLogger(LogLevel level, IMessagePreparer preparer, TextWriter writerToWriteTo)
		{
			State = level;

			if (writerToWriteTo == null)
				throw new ArgumentNullException("writerToWriteTo", "This logger requires a valid TextWriter, non-null, to write to.");

			if (preparer == null)
				throw new ArgumentNullException("preparer", "This logger requires a valid " + typeof(IMessagePreparer).ToString() + " to prepare messages with.");

			writer = writerToWriteTo;
			messagePreparer = preparer;
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
			if (State.HasFlag(level)) //If we're not including that LogLevel then we don't queue up this log request.
			{
				string toLog = messagePreparer.Prepare(new LogMessage(log, level, caller, objs));
				LogString(toLog);
			}	
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
			if (State.HasFlag(level)) //If we're not including that LogLevel then we don't queue up this log request.
			{
				string toLog = messagePreparer.Prepare(new LogMessage(obj, level, caller, objs));
				LogString(toLog);
			}	
		}
		#endregion

		private void LogString(string message)
		{
			writer.WriteLine(message);
		}

		public void Dispose()
		{
			messagePreparer.Dispose();
		}
    }
}
