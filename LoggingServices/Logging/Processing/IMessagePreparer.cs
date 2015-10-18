using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Services
{
	public interface IMessagePreparer : IDisposable
	{
		string Prepare(ILogMessage message);
	}
}
