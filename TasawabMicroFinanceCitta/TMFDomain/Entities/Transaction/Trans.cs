using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Accounts;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Transaction
{
    public class Trans : BaseEntity
    {
        public int Id { get; set; }
        public string AccountCode { get; set; } // Foreign key to Account
        public string TransactionType { get; set; } // "Deposit", "Withdrawal"
        public decimal Amount { get; set; }
        public decimal Charges { get; set; }
        public decimal FinalAmount { get; set; } // Amount after charges
        public string Reference { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; } // Staff who performed the transaction

        // Navigation property
        public AccountEntity Account { get; set; }
    }
}