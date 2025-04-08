using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Loan.Approval
{
    public class ApprovalSetup : BaseEntity
    {
        public string ApprovalSetupId { get; set; }
        public string RoleId { get; set; }
        public int ApprovalOrder { get; set; } // Order of approval (e.g., 1 = First approver)
    }
}
