using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otpCode);
    }
}
