using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Accounts
{
    public class AccountEntityType : BaseEntity
    {
        public string EntityTypeCode { get; set; }
        public string Name { get; set; } // e.g., "Single", "Joint", "Corporate"
        public int MinOwners { get; set; } = 1;
        public int MaxOwners { get; set; } = 1;
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
