using APIs.Contract;
using APIs.ViewModel.Others;
using System.Net.Mail;

namespace APIs.Utilities
{
    public class MailService : IEmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string fromEmailAddress;

        private FluentEmailVM fluentEmail = new();

        public MailService(string smtpServer, int smtpPort, string fromEmailAddress)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.fromEmailAddress = fromEmailAddress;
        }

        public MailService SetEmail(string email)
        {
            fluentEmail.Email = email;
            return this;
        }

        public MailService SetSubject(string subject)
        {
            fluentEmail.Subject = subject;
            return this;
        }

        public MailService SetHtmlMessage(string htmlMessage)
        {
            fluentEmail.HtmlMessage = htmlMessage;
            return this;
        }

        /*
         * <summary>
         * Send email
         * </summary>
         * <param name="email">Email address</param>
         * <param name="subject">Email subject</param>
         * <param name="htmlMessage">Email message</param>
         * <returns>Task</returns>
         * <remarks>
         * This method will send email to email address
         * </remarks>
         */
        public void SendEmailAsync()
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromEmailAddress),
                Subject = fluentEmail.Subject,
                Body = fluentEmail.HtmlMessage,
                To = { fluentEmail.Email },
                IsBodyHtml = true
            };

            using var client = new SmtpClient(smtpServer, smtpPort);
            client.Send(message);

            message.Dispose();
            client.Dispose();
        }
    }
}
