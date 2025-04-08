using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Client;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Accounts
{
    public class AccountOwner : BaseEntity
    {
        public int Id { get; set; }
        public string AccountCode { get; set; } // Foreign key to Account
        public int ClientId { get; set; } // Foreign key to ClientInformation
        public string OwnershipType { get; set; } // "Primary", "Secondary", etc.
        public decimal OwnershipPercentage { get; set; } // For joint accounts

        // Navigation properties
        public AccountEntity Account { get; set; }
        public ClientInformation Client { get; set; }
    }
}
