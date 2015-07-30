using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
	public class ChainLogger : ILoggingService
	{
		public LogLevel State
		{
			get { return loggerChain.Aggregate(LogLevel.Error, (c, x) => c | x.State); }
		}

		private readonly IEnumerable<ILogger> loggerChain;

		public ChainLogger(IEnumerable<ILogger> loggers)
		{
			loggerChain = loggers;
		}

		public void Log(string log, LogLevel level)
		{
			foreach (var l in loggerChain)
				l.Log(log, level);
		}

		public void Log(string log, LogLevel level, Type caller)
		{
			foreach (var l in loggerChain)
				l.Log(log, level, caller);
		}

		public void Log(string log, LogLevel level, params object[] objs)
		{
			foreach (var l in loggerChain)
				l.Log(log, level, objs);
		}

		public void Log(string log, LogLevel level, Type caller, params object[] objs)
		{
			foreach (var l in loggerChain)
				l.Log(log, level, caller, objs);
		}

		public void Log(object obj, LogLevel level)
		{
			foreach (var l in loggerChain)
				l.Log(obj, level);
		}

		public void Log(object obj, LogLevel level, Type caller)
		{
			foreach (var l in loggerChain)
				l.Log(obj, level, caller);
		}

		public void Log(object obj, LogLevel level, params object[] objs)
		{
			foreach (var l in loggerChain)
				l.Log(obj, level, objs);
		}

		public void Log(object obj, LogLevel level, Type caller, params object[] objs)
		{
			foreach (var l in loggerChain)
				l.Log(obj, level, caller, objs);
		}
	}
}
