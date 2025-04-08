using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Loan
{
    public class LoanType : BaseEntity
    {
        public string LoanTypeId { get; set; }
        public string LoanName { get; set; } 
        public string LoanDescription { get; set; } 
        public decimal InterestRate { get; set; }
        public decimal PenaltyRate { get; set; }
        public int TenureYears { get; set; }
        public string InstallmentFrequency { get; set; } // change to numbers 1,2,3,4 for codes
        public bool IsActive { get; set; } = true;
        public string PerformedBy { get; set; }
    }
}
