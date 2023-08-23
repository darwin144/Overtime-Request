using APIs.Utilities;

namespace APIs.Contract
{
    public interface IEmailService
    {
        void SendEmailAsync();
        MailService SetEmail(string email);
        MailService SetSubject(string subject);
        MailService SetHtmlMessage(string htmlMessage);
    }
}
