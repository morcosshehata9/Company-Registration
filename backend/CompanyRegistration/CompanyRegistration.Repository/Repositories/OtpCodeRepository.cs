using CompanyRegistration.Data.Models;
using CompanyRegistration.Repository.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Repository.Repositories
{
    public class OtpCodeRepository : GenericRepository<OtpCode, Guid>, IOtpCodeRepository
    {
        private readonly AppDbContext _context;

        public OtpCodeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OtpCode> GetLatestByEmailAsync(string email)
        {
            return await _context.OtpCodes
                .Where(o => o.Email == email && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt) // أو o.Id لو بيعبر عن الترتيب
                .FirstOrDefaultAsync();
        }

    }
}
