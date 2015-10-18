using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Logging.Services
{
	/// <summary>
	/// Contracts a class/interface to become a string message producer/provider. 
	/// Provides <see cref="ICollection<>"/> functionality and a single blocking method to get a <see cref="string"/> message.
	/// </summary>
	public interface IMessagePreparer : ICollection<ILogMessage>, IDisposable
	{
		/// <summary>
		/// Provides a prepared <see cref="string"/> generated from a message.
		/// This may block.
		/// </summary>
		/// <returns>Returns a prepared string. (Blocks)</returns>
		string Take(); //Will block
	}
}
