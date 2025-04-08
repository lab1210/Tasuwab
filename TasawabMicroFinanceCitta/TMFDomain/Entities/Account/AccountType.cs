using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Accounts
{
    public class AccountType : BaseEntity
    {
        public string AccountTypeCode { get; set; }
        public string Name { get; set; } // e.g., "Savings", "Current"
        public decimal InterestRate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
