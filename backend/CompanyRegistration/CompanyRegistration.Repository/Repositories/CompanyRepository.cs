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
    public class CompanyRepository : GenericRepository<Company, Guid>, ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Company> GetCompanyByEmailAsync(string email)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted);

        }
    }
}
