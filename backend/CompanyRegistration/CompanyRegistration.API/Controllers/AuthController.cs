using CompanyRegistration.Data.DTOs;
using CompanyRegistration.Repository.UOW;
using CompanyRegistration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyRegistration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(ICompanyService companyService, IUnitOfWork unitOfWork)
        {
            _companyService = companyService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] CompanySignUpDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _companyService.SignUpAsync(dto);
            return Ok(new { message = "OTP sent to your email." });
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] OtpVerificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.ValidateOtpAsync(dto.Email, dto.OtpCode);
            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP." });

            return Ok(new { message = "OTP verified successfully." });
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            await _companyService.SetPasswordAsync(dto);
            return Ok(new { message = "Password set successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            var token = await _companyService.LoginAsync(dto);
            if (token == null)
                return Unauthorized(new { message = "Invalid credentials." });

            return Ok(new { token });
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {

            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(claimValue, out var companyId))
                return Unauthorized();

            var company = await _unitOfWork.Companies.GetByIdAsync(companyId);

            if (company == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var logoUrl = string.IsNullOrEmpty(company.LogoPath)
                ? null
                : $"{baseUrl}/{company.LogoPath.Replace("\\", "/")}";

            return Ok(new
            {
                company.Id,
                company.ArabicName,
                company.EnglishName,
                company.Email,
                company.PhoneNumber,
                LogoPath= logoUrl,
            });
        }
    }
}

