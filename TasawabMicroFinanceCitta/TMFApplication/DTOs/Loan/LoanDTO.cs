using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFApplication.DTOs.Loan
{
    public class UpdateLoanTypeRequest
    {
        public string LoanName { get; set; }
        public string LoanDescription { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PenaltyRate { get; set; }
        public int TenureYears { get; set; }
        public string InstallmentFrequency { get; set; }
    }
    public class AddLoanTypeRequest
    {
        public string LoanName { get; set; }
        public string LoanDescription { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PenaltyRate { get; set; }
        public int TenureYears { get; set; }
        public string InstallmentFrequency { get; set; }
    }

    public class CreateLoanApplicationRequest
    {
        public string AccountCode { get; set; }
        public string LoanTypeId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public List<string> DocumentLinks { get; set; } // List of document links
    }

    public class ApproveLoanApplicationRequest
    {
        public string LoanApplicationId { get; set; }
        public string RoleId { get; set; }
    }
}
