using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Services
{
	public class RecyclingMessagePreparer : IMessagePreparer
	{
		private readonly IThreadedAccessContainer<StringBuilder> stringBuilderServiceProvider;

		public RecyclingMessagePreparer(IThreadedAccessContainer<StringBuilder> stringbuilderProvider)
		{
			stringBuilderServiceProvider = stringbuilderProvider;
		}

		public string Prepare(ILogMessage message)
		{
			using(var sb = stringBuilderServiceProvider.Get())
			{
				return message.BuildMessage(sb.Get());
			}
		}

		public void Dispose()
		{
			//Nothing to dispose
		}
	}
}
