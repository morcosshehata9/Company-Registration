using CompanyRegistration.Data.DTOs;
using CompanyRegistration.Data.Models;
using CompanyRegistration.Repository.UOW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyRegistration.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpCodeService _otpCodeService;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailSender;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IConfiguration _configuration;

        public CompanyService(IUnitOfWork unitOfWork,
                              IOtpCodeService otpCodeService,
                              IFileService fileService,
                              IEmailService emailSender,
                              IPasswordHasherService passwordHasher,
                              IConfiguration configuration)

        {
            _unitOfWork = unitOfWork;
            _otpCodeService = otpCodeService;
            _fileService = fileService;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }


        public async Task SignUpAsync(CompanySignUpDto dto)
        {

            var existing = await _unitOfWork.Companies.GetCompanyByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("This email is already registered.");

            string? logoPath = null;
            if (dto.Logo != null)
                logoPath = await _fileService.SaveFileAsync(dto.Logo, "logos");


            var newCompany = new Company
            {
                Id = Guid.NewGuid(),
                ArabicName = dto.NameAr,
                EnglishName = dto.NameEn,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                WebsiteUrl = dto.WebsiteUrl,
                LogoPath = logoPath,
                IsDeleted=false,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow,
            };



            await _unitOfWork.Companies.AddAsync(newCompany);
            await _unitOfWork.SaveAsync();

            var otp = await _otpCodeService.GenerateOtpAsync(dto.Email);
            await _emailSender.SendOtpAsync(dto.Email, otp.Code);
        }
        
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var company = await _unitOfWork.Companies.GetCompanyByEmailAsync(dto.Email);
            if (company == null || string.IsNullOrEmpty(company.PasswordHash))
                return null;

            var isValidPassword = _passwordHasher.VerifyPassword(company.PasswordHash, dto.Password);
            if (!isValidPassword)
                return null;

            // Generate JWT
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, company.Id.ToString()),
                new Claim(ClaimTypes.Email, company.Email ?? string.Empty),
                new Claim("CompanyEnglishName", company.EnglishName ?? string.Empty),
                new Claim("CompanyArabicName", company.ArabicName ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task SetPasswordAsync(SetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("Passwords do not match.");

            var company = await _unitOfWork.Companies.GetCompanyByEmailAsync(dto.Email);
            if (company == null || !company.IsVerified)
                throw new InvalidOperationException("Email not confirmed or company not found.");

            company.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ValidateOtpAsync(string email, string otpCode)
        {
            var isValid = await _otpCodeService.IsValidOtpAsync(email, otpCode);
            if (!isValid) return false;

            var company = await _unitOfWork.Companies.GetCompanyByEmailAsync(email);
            if (company == null) return false;

            company.IsVerified = true;
            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveAsync();

            return true;

        }
    }
}
