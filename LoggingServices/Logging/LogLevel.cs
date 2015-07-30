using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
	/// <summary>
	/// Flags enum that indicates the logging level of something.
	/// </summary>
	[Flags]
	public enum LogLevel : byte
	{
		Error = 1, //Error logs
		Warning = 1 << 1, //Warning logs
		Debug = 1 << 2, //Debugging logs
		All = Error | Warning | Debug, //Bitwise or of all message types
	}
}
