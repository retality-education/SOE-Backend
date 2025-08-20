using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using Application.Interfaces.EmailSender;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;

namespace Infrastructure.RestorePasswordEmail.EmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient _client;
        private readonly EmailOptions _options;

        public SendGridEmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value;
            _client = new SendGridClient(_options.SendGridApiKey);
        }

        public async Task<bool> SendPasswordResetCodeAsync(string email, string resetCode, string userName)
        {
            var subject = "Восстановление пароля";
            var htmlContent = $@"
                <h2>Восстановление пароля</h2>
                <p>Здравствуйте, {userName}!</p>
                <p>Ваш код для восстановления пароля: <strong>{resetCode}</strong></p>
                <p>Код действителен в течение 15 минут.</p>
                <p>Если вы не запрашивали восстановление пароля, проигнорируйте это письмо.</p>
            ";

            return await SendEmailAsync(email, subject, htmlContent);
        }

        private async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            try
            {
                var from = new EmailAddress(_options.FromEmail, _options.FromName);
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var response = await _client.SendEmailAsync(msg);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
