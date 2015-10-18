using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Logging.Services
{
	/// <summary>
	/// Container for a <see cref="StringBuilder"/> that acts as a service producer.
	/// It will provider a StringBuilder on demand that may or may not be the same instance every call.
	/// </summary>
	public class StringBuilderContainer : IThreadedAccessContainer<StringBuilder>
	{
		/// <summary>
		/// Provides a Lazy loaded Recylable instance of a StringBuilder for this container to provide to callers.
		/// </summary>
		private readonly Lazy<Recyler<StringBuilder>> builder;
		private readonly object syncObj = new object();

		public StringBuilderContainer()
		{
			builder = new Lazy<Recyler<StringBuilder>>(BuildForLazyInit, true);
		}

		/// <summary>
		/// Indicates if an instance is available.
		/// </summary>
		public bool isAvailable
		{
			//We can always provider a new instance.
			get { return true; }
		}

		/// <summary>
		/// Produces an instance of <see cref="StringBuilder"/> in a wrapper that implements <see cref="IDisposable"/>
		/// that must be disposed of after use.
		/// </summary>
		/// <returns>An instance of <see cref="Recycler<StringBuilder>"/> for the caller to use.</returns>
		public Recyler<StringBuilder> Get()
		{
			if (Monitor.TryEnter(syncObj))
			{
				try
				{
					return builder.Value;
				}
				catch(Exception e) //Don't do a finally. If you do a finally with exit the exit called in the future will throw obviously. We only want to exit if there is an issue with returning.
				{
					Monitor.Exit(syncObj);
					throw; //Rethrow because this means we might have lost our StringBuilder access.
				}
			}
			else
			{
				//If we can't get a monitor lock on the object we need to produce one.
				//This can happen in scenarios were multiple threads are accessing this resource but not if called multiple times on the same thread before disposal
				return new Recyler<StringBuilder>(null, new StringBuilder());
			}

		}

		private Recyler<StringBuilder> BuildForLazyInit()
		{
			//Don't use lambda for readability in the lazy init above.
			return new Recyler<StringBuilder>((sb) =>
			{
				//Kind of dangerous since it's possible execution may never come back to this due to exceptions.
				Monitor.Exit(this.syncObj); //syncObj is captured in this lambda from the scope of this class.

				if(sb != null) //there will likely be issues elsewhere if this happens to be false so maybe we shouldn't check it.
					sb.Clear();
	
			}, new StringBuilder());
		}
	}
}
