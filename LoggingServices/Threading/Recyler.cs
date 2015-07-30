using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
	/// <summary>
	/// Wrapper for a recyleable resource. Encapsulates a function for recycling by abusing the IDiposable pattern.
	/// This should be disposed after use.
	/// </summary>
	/// <typeparam name="ItemType"></typeparam>
	public class Recyler<ItemType> : IDisposable //This may be abuse of IDiposable Pattern
	{
		/// <summary>
		/// Provides a callback to dispose of the ItemType instance through.
		/// </summary>
		private readonly Action<ItemType> recycleAction;

		/// <summary>
		/// Holds the instance of ItemType that is recycleable.
		/// </summary>
		private readonly ItemType _InstanceOfType;

		public Recyler(Action<ItemType> action, ItemType instance)
		{
			recycleAction = action;
			_InstanceOfType = instance;
		}

		public void Dispose()
		{
			//We could check if ItemType implements IDiposable however we don't know if we own the instance.

			//If it's null we can assume that no action is to be taken.
			if(recycleAction != null)
			{
				recycleAction.Invoke(_InstanceOfType);
			}
		}

		/// <summary>
		/// Produces an instance object of ItemType.
		/// </summary>
		/// <returns>An instance of ItemType</returns>
		public ItemType Get()
		{
			return _InstanceOfType;
		}
	}
}
