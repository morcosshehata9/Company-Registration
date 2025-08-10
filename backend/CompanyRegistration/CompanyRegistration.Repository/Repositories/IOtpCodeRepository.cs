using CompanyRegistration.Data.Models;
using CompanyRegistration.Repository.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Repository.Repositories
{
    public interface IOtpCodeRepository: IGenericRepository<OtpCode, Guid>
    {
        Task<OtpCode> GetLatestByEmailAsync(string email);
    }
}
