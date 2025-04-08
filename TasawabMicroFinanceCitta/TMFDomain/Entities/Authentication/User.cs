using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Authentication
{
    public class User : BaseEntity
    {
        public string? staff_code { get; set; } = string.Empty;
        public string? email { get; set; } = string.Empty;
        public string? role_code { get; set; } = string.Empty;
        public string? position_code { get; set; } = string.Empty;
        public string? department_code { get; set; } = string.Empty;
        public string? branch_code { get; set; } = string.Empty;
        public string? password { get; set; } = string.Empty;
        public bool IsPasswordSet { get; set; } = false;
        public bool? is_active { get; set; } = false;
        public bool? IsVisible { get; set; } = true;
    }
}
