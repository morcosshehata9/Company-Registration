using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Data.DTOs
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public string CompanyName { get; set; }
        public string LogoPath { get; set; }
    }
}
