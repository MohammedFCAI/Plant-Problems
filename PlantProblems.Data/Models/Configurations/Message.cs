using MimeKit;

namespace Plant_Problems.Data.Models.Configurations
{
	public class Message
	{

		public List<MailboxAddress> To { get; set; }

		public string Subject { get; set; }

		public string Content { get; set; }

		public Message(IEnumerable<string> to, string subject, string content)
		{
			To = new List<MailboxAddress>();
			To.AddRange(to.Select(i => new MailboxAddress("email", i)));
			Subject = subject;
			Content = content;
		}

		public Message(string to, string subject, string content)
		{
			var toEnumerable = ConvertStringToEnumerable(to);
			To = new List<MailboxAddress>();
			To.AddRange(toEnumerable.Select(i => new MailboxAddress("email", i)));
			Subject = subject;
			Content = content;
		}

		private IEnumerable<string> ConvertStringToEnumerable(string input)
		{
			// Split the string by commas and trim the whitespace from each item
			return input.Split(',').Select(item => item.Trim());
		}
	}
}
