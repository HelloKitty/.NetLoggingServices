using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Services
{
	/// <summary>
	/// Interface that can contract a class to become a producer of a particular Type.
	/// The interface also implies, but doesn't contratually enforce, that threaded access is safe and supported.
	/// </summary>
	/// <typeparam name="ItemType">Type of the object instance to be held within the container.</typeparam>
	public interface IThreadedAccessContainer<ItemType>
	{
		/// <summary>
		/// Indicates if the objects is avaiable for accessing. Calling this and then getting the object is not threadsafe.
		/// </summary>
		bool isAvailable { get; } //We can't promise this to be atomic with getting the resource

		/// <summary>
		/// Retrieves an instance of Type T from the container for use.
		/// </summary>
		/// <returns></returns>
		Recyler<ItemType> Get();
	}
}
