using System;
namespace SVIBEL.Core.Models.Messaging
{
	public class MessagingConfig : IConfig
	{
		public string Host { get; set; }
		public string UnauthorizedRequestTopic { get; set; }
		public string ExchangeName { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public int Port { get; set;}
		public string ConnectionString { get { return string.Format("host={0}:{1};virtualhost={2};username={3};password={4}", Host, Port, "/", User, Password); } }


		public bool IsEditable { get; set; }
		public string Id { get; set; }
	}
}
