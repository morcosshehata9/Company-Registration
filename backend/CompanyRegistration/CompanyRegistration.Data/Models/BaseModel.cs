﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Data.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }= false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
