using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Staff
{
    public class Privilege : BaseEntity
    {
        [Key]
        public string PrivilegeId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
