using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
	/// <summary>
	/// Implements the <see cref="ILogMessage"/> inferace which contracts this class to encapsulate the basics of a loggable message.
	/// This class also, through the interface, provides functionality to serialize the information of this class into a string.
	/// </summary>
	public class LogMessage : ILogMessage
	{
		/// <summary>
		/// Represents the main message object of the <see cref="LogMessage"/>.
		/// </summary>
		public object MainMessageObject { get; private set; }

		private Type _CallingType;
		/// <summary>
		/// Represents the <see cref="Type"/> of the caller of this <see cref="LogMessage"/>.
		/// </summary>
		public Type CallingType
		{
			get { return _CallingType == null ? (_CallingType = typeof(object)) : _CallingType; }
			private set { _CallingType = value; }
		}

		private object[] _ObjParams;
		/// <summary>
		/// Represents an optional array of formatting params that can be used to format the message.
		/// </summary>
		public object[] ObjParams
		{
			get { return _ObjParams == null ? (_ObjParams = new object[0]) : _ObjParams; }
			private set { _ObjParams = value; }
		}

		/// <summary>
		/// Represents the <see cref="LogLevel"/> of this <see cref="LogMessage"/>.
		/// </summary>
		public LogLevel Level { get; private set; }

		/// <summary>
		/// Constructs an immutable LogMessage given the parameters that can be built into a string message.
		/// </summary>
		/// <param name="message">Main message for the <see cref="LogMessage"/>.</param>
		/// <param name="lvl">The desired <see cref="LogLevel"/> of the message.</param>
		/// <param name="callerType">The caller <see cref="Type"/> of this message.</param>
		/// <param name="objsOptional">Optional formatting parameters for this message.</param>
		public LogMessage(string message, LogLevel lvl, Type callerType, params object[] objsOptional) 
			: this((object)message, lvl, callerType, objsOptional)
		{
			//Just call the other constructor. Casting to object is important because otherwise you'll get recusive calls and overflow.
		}

		/// <summary>
		/// Constructs an immutable LogMessage given the parameters that can be built into a string message.
		/// </summary>
		/// <param name="message">Main message object for the <see cref="LogMessage"/>.</param>
		/// <param name="lvl">The desired <see cref="LogLevel"/> of the message.</param>
		/// <param name="callerType">The caller <see cref="Type"/> of this message.</param>
		/// <param name="objsOptional">Optional formatting parameters for this message.</param>
		public LogMessage(object messageObject, LogLevel lvl, Type callerType, params object[] objsOptional)
		{
			MainMessageObject = messageObject;
			CallingType = callerType;
			ObjParams = objsOptional;
			Level = lvl;
		}

		/// <summary>
		/// Produces a string message representing the <see cref="LogMessage"/>.
		/// This method implements its own way to generate the message.
		/// </summary>
		/// <returns>A string representing <see cref="LogMessage"/>.</returns>
		public string BuildMessage()
		{
			StringBuilder sb = new StringBuilder();
			return BuildMessage(sb);
		}

		private string BuildMessageWithParams(StringBuilder sb)
		{
			try
			{
				return sb.AppendFormat(BuildMessageNoParams(), _ObjParams).ToString();
			}
			catch(NullReferenceException e)
			{
				//Most likely the SB was null.
#if DEBUG || DEBUGBUILD
				Console.WriteLine("error in building log string. Null SB");
#endif
				throw;
			}
		}

		private string BuildMessageNoParams()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Level);
			sb.Append(": ");
			sb.Append(MainMessageObject);
			sb.Append(" Called by: ");
			sb.Append(CallingType);

			return sb.ToString();
		}

		/// <summary>
		/// Produces a string message representing the <see cref="LogMessage"/>.
		/// This method uses the provided <see cref="StringBuilder"/> to generate the message.
		/// </summary>
		/// <returns>A string representing <see cref="LogMessage"/>.</returns>
		public string BuildMessage(StringBuilder sb)
		{
			if (_ObjParams == null)
				return BuildMessageNoParams();
			else
				return BuildMessageWithParams(sb);
		}
	}
}
