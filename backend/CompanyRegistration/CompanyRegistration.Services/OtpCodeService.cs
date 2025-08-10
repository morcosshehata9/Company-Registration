using CompanyRegistration.Data.Models;
using CompanyRegistration.Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services
{
    public class OtpCodeService: IOtpCodeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OtpCodeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OtpCode> GenerateOtpAsync(string email)
        {
            var otp = new OtpCode
            {
                Id = Guid.NewGuid(),
                Email = email,
                Code = GenerateOtpCode(),
                ExpiryDate = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                
            };

            await _unitOfWork.OtpCodes.AddAsync(otp);
            await _unitOfWork.SaveAsync();

            return otp;
        }

        public async Task<OtpCode?> GetLatestOtpAsync(string email)
        {
            return await _unitOfWork.OtpCodes.GetLatestByEmailAsync(email);
        }

        public async Task<bool> IsValidOtpAsync(string email, string otpCode)
        {
            var otp = await GetLatestOtpAsync(email);

            if (otp == null)
                return false;

            return otp.Code == otpCode && otp.ExpiryDate > DateTime.UtcNow;
        }

        private string GenerateOtpCode()
        {
            int code = RandomNumberGenerator.GetInt32(100000, 1000000);
            return code.ToString("D6");
        }
    }
}
