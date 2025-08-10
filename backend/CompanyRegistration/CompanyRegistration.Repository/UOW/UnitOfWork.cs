using CompanyRegistration.Data.Models;
using CompanyRegistration.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Repository.UOW
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICompanyRepository Companies { get; private set; }
        public IOtpCodeRepository OtpCodes { get; private set; }
    

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Companies = new CompanyRepository(_context);
            OtpCodes = new OtpCodeRepository(_context); 
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            
        }
    }
}
