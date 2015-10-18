using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logging.Services
{
	/// <summary>
	/// An interface that contracts logging service functionality.
	/// Allows depending objects to log objects or strings in various ways that include support for formatting.
	/// </summary>
	public interface ILogger
	{
        LogLevel State { get; }

		//String logging methods
		#region string based logging
		/// <summary>
		/// Logs a <see cref="string"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="string"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		void Log(string log, LogLevel level);

		/// <summary>
		/// Logs a <see cref="string"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="string"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="caller"><see cref="Type"/> of the caller for logging.</param>
		void Log(string log, LogLevel level, Type caller);

		/// <summary>
		/// Logs a <see cref="string"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="string"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="objs"><see cref="object"/> optional array of objects to be used in log formatting.</param>
		void Log(string log, LogLevel level, params object[] objs);


		/// <summary>
		/// Logs a <see cref="string"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="string"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="caller"><see cref="Type"/> of the caller for logging.</param>
		/// <param name="objs"><see cref="object"/> optional array of objects to be used in log formatting.</param>
		void Log(string log, LogLevel level, Type caller, params object[] objs);
		#endregion

		//Object logging methods
		#region object-based logging
		/// <summary>
		/// Logs an <see cref="object"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="object"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		void Log(object obj, LogLevel level);

		/// <summary>
		/// Logs an <see cref="object"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="object"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="caller"><see cref="Type"/> of the caller for logging.</param>
		void Log(object obj, LogLevel level, Type caller);

		/// <summary>
		/// Logs an <see cref="object"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="object"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="objs"><see cref="object"/> optional array of objects to be used in log formatting.</param>
		void Log(object obj, LogLevel level, params object[] objs);

		/// <summary>
		/// Logs an <see cref="object"/> with the indicated <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="log"><see cref="object"/> to log.</param>
		/// <param name="level"><see cref="LogLevel"/> to log as.</param>
		/// <param name="caller"><see cref="Type"/> of the caller for logging.</param>
		/// <param name="objs"><see cref="object"/> optional array of objects to be used in log formatting.</param>
		void Log(object obj, LogLevel level, Type caller, params object[] objs);
		#endregion
	}
}
