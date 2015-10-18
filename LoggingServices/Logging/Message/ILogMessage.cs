using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logging.Services
{
	/// <summary>
	/// A contract that implies an object is a loggable message that contains various information about a logging request.
	/// </summary>
	public interface ILogMessage
	{
		/// <summary>
		/// Represents the main message component of a log message/request.
		/// </summary>
		object MainMessageObject { get; }

		/// <summary>
		/// Indicates the <see cref="Type"/> of the caller for this message/request.
		/// </summary>
		Type CallingType { get; }

		/// <summary>
		/// Optional <see cref="object"/> array containing objects used in formatting the message/request.
		/// </summary>
		object[] ObjParams { get; }

		/// <summary>
		/// Indicates the <see cref="LogLevel"/> of the log message/request.
		/// </summary>
		LogLevel Level { get; }

		/// <summary>
		/// Builds and produces a string representing the LogMessage.
		/// </summary>
		/// <returns>A string that represents the log message in a formatted fashion depending on implementation.</returns>
		string BuildMessage();

		/// <summary>
		/// Builds with a provided StringBuilder and produces a string representing the LogMessage.
		/// </summary>
		/// <param name="sb">A <see cref="StringBuilder"/> to be used in the formatting process.</param>
		/// <returns>A string that represents the log message in a formatted fashion depending on implementation.</returns>
		string BuildMessage(StringBuilder sb);
	}
}
