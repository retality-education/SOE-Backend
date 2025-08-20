using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.EmailSender
{
    public interface IEmailSender
    {
        Task<bool> SendPasswordResetCodeAsync(string email, string resetCode, string userName);
        /* 
         * идейно, мне хочется вынести класс с отправкой сообщения в отдельный, а отправку конкретного письма
         * в отдельное место, т.к. можно добавить логику отправки других писем и тогда придётся добавлять функционал сюда, мб partial, но в чём разница
         * В общем пока решение, что сделаем метод отправки письма приватным и сделаем публичные методы конкретного письма, как сборщик писем
         */
        //Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
}
