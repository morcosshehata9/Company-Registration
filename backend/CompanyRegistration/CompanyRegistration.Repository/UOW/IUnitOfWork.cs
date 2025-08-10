using CompanyRegistration.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Repository.UOW
{
    public interface IUnitOfWork: IDisposable
    {
        ICompanyRepository Companies { get; }
        IOtpCodeRepository OtpCodes { get; }
        Task<int> SaveAsync();
    }
}
