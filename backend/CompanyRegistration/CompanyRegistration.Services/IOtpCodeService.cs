using CompanyRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services
{
    public interface IOtpCodeService
    {
        Task<OtpCode> GenerateOtpAsync(string email);
        Task<OtpCode?> GetLatestOtpAsync(string email);
        Task<bool> IsValidOtpAsync(string email, string otpCode);
    }
}
