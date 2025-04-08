using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Loan.Approval;
using TMFDomain.Entities.Loan;

namespace TMFDomain.Interfaces.Loan
{
    public interface ILoanService
    {
        // Loan Type
        Task<(bool success, string message)> AddLoanTypeAsync(LoanType loanType, string performedBy);
        Task<(bool success, string message)> UpdateLoanTypeAsync(LoanType loanType, string performedBy);
        Task<IEnumerable<LoanType>> GetAllLoanTypesAsync();

        // Loan Application
        Task<(bool success, string message)> CreateLoanApplicationAsync(LoanApplication loanApplication, string performedBy);
        Task<(bool success, string message)> UpdateLoanApplicationAsync(LoanApplication loanApplication, string performedBy);
        Task<LoanApplication> GetLoanApplicationByIdAsync(string loanApplicationId);

        // Approval
        Task<(bool success, string message)> ApproveLoanApplicationAsync(string loanApplicationId, string roleId, string performedBy);
        Task<(bool success, string message)> RejectLoanApplicationAsync(string loanApplicationId, string roleId, string performedBy);
        Task<IEnumerable<ApprovalTracking>> GetApprovalTrackingAsync(string loanApplicationId);
    }
}