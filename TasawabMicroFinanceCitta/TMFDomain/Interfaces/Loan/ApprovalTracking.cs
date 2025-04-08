using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Interfaces.Loan
{
    public class ApprovalTracking : BaseEntity 
    {
        public string ApprovalTrackingId { get; set; }
        public string LoanApplicationId { get; set; }
        public string RoleId { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
    }
}
