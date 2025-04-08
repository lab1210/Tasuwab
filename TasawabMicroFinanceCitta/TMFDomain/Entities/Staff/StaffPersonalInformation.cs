using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Staff
{
    public class StaffPersonalInformation : BaseEntity
    {
        public string? staff_code { get; set; } = string.Empty;
        public string? first_name { get; set; } = string.Empty;
        public string? last_name { get; set; } = string.Empty;
        public string? gender { get; set; } = string.Empty;
        public string? martial_status { get; set; } = string.Empty;
        public int? date_of_birth { get; set; } // this is going to be YYYYMMDD
        public string? address { get; set; } = string.Empty;
        public string? email { get; set; } = string.Empty;
        public string? phone { get; set; } = string.Empty;
        public string? staff_image { get; set; }
    }

}