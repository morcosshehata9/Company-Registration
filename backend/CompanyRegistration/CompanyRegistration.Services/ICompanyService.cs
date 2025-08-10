using CompanyRegistration.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services
{
    public interface ICompanyService
    {
        Task SignUpAsync(CompanySignUpDto dto);
        Task<bool> ValidateOtpAsync(string email, string otpCode);
        Task SetPasswordAsync(SetPasswordDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
