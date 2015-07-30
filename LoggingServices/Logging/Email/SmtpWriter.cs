using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
	public class SmtpWriter : TextWriter
	{
		public override Encoding Encoding
		{
			get { return Encoding.ASCII; }
		}

		private readonly ISmtpCredentials credentials;
		private readonly ISmtpHeader header;

		public SmtpWriter(ISmtpCredentials cred, ISmtpHeader head)
		{
			credentials = cred;
			header = head;
		}

		public override void Write(char value)
		{
			Write(value.ToString());
		}

		public override void Write(string value)
		{
			//We wait because the caller is expecting this to be sycronous.
			this.WriteLineAsync(value).Wait();
		}

		public override Task WriteLineAsync(string value)
		{
			using (MailMessage message = new MailMessage(header.From, header.To, header.Subject, value))
			{
				using (SmtpClient client = new SmtpClient(credentials.HostName, credentials.Port))
				{
					client.DeliveryMethod = SmtpDeliveryMethod.Network;
					client.EnableSsl = true;

					return client.SendMailAsync(message);
				}
			}
		}

		public override Task WriteAsync(string value)
		{
			return WriteLineAsync(value);	
		}
	}
}
