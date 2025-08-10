using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Data.Models
{
    public class OtpCode: BaseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
    }
}
