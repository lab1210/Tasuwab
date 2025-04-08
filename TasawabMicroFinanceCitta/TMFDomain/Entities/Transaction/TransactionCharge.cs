using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Transaction
{
    public class TransactionCharge : BaseEntity
    {
        public int Id { get; set; }
        public string TransactionType { get; set; } // "Deposit", "Withdrawal"
        public string ChargeType { get; set; } // "Fixed", "Percentage"
        public decimal Value { get; set; }
        public decimal MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
