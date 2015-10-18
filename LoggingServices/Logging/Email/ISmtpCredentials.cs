using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logging.Services
{
    public interface ISmtpCredentials
    {
        string UserName { get; }
        string Password { get; }
        string HostName { get; }
		int Port { get; }
    }
}
