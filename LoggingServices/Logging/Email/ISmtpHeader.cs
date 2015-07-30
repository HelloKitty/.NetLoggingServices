using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
    public interface ISmtpHeader
    {
        string Subject { get; }
        string From { get; }
        string To { get; }
    }
}
