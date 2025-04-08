using System;
using System.Collections.Generic;
using TMFDomain.Shared;
using TMFDomain.Entities.Transaction;

namespace TMFDomain.Entities.Accounts
{
    public class AccountEntity : BaseEntity
    {
        public string AccountCode { get; set; } // Primary key
        public string AccountTypeCode { get; set; } // Foreign key to AccountType
        public string EntityTypeCode { get; set; } // Foreign key to AccountEntityType
        public decimal Balance { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string BranchCode { get; set; }

        // Navigation properties
        public AccountType AccountType { get; set; }
        public AccountEntityType AccountEntityType { get; set; }
        public ICollection<AccountOwner> AccountOwners { get; set; }
        public ICollection<Trans> Transactions { get; set; }
    }
}