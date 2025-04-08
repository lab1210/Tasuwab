using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Loan.Approval;
using TMFDomain.Entities.Loan;

namespace TMFDomain.Interfaces.Loan
{
    public interface ILoanRepository : IRepository<LoanApplication>
    {
        // Loan Type
        Task AddLoanTypeAsync(LoanType loanType);
        Task UpdateLoanTypeAsync(LoanType loanType);
        Task<IEnumerable<LoanType>> GetAllLoanTypesAsync();

        // Loan Application
        Task AddLoanApplicationAsync(LoanApplication loanApplication);
        Task UpdateLoanApplicationAsync(LoanApplication loanApplication);
        Task<LoanApplication> GetLoanApplicationByIdAsync(string loanApplicationId);

        // Approval
        Task AddApprovalTrackingAsync(ApprovalTracking tracking);
        Task<IEnumerable<ApprovalTracking>> GetApprovalTrackingByLoanApplicationIdAsync(string loanApplicationId);
    }
}