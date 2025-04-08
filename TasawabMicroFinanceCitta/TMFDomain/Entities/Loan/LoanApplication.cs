using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Loan
{
    public class LoanApplication : BaseEntity
    {
        public string LoanApplicationId { get; set; }
        public string LoanApprovalId { get; set; }
        public string AccountCode { get; set; }
        public string LoanTypeId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public int ApprovalStatus { get; set; } 
        public string DocumentLinks { get; set; } 
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; } 
        public string PerformedBy { get; set; } 
    }

}
