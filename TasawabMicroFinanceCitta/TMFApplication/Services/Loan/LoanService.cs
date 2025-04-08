using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Loan;
using TMFDomain.Interfaces.Loan;

namespace TMFApplication.Services.Loan
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }


        public async Task<(bool success, string message)> UpdateLoanTypeAsync(LoanType loanType, string performedBy)
        {
            try
            {
                loanType.PerformedBy = performedBy; 
                loanType.updated_at = DateTime.Now; 
                await _loanRepository.UpdateLoanTypeAsync(loanType);
                return (true, "Loan type updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<LoanType>> GetAllLoanTypesAsync()
        {
            return await _loanRepository.GetAllLoanTypesAsync();
        }

        // Loan Application
        public async Task<(bool success, string message)> CreateLoanApplicationAsync(LoanApplication loanApplication, string performedBy)
        {
            try
            {
                loanApplication.PerformedBy = performedBy;
                await _loanRepository.AddLoanApplicationAsync(loanApplication);
                return (true, "Loan application created successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string message)> UpdateLoanApplicationAsync(LoanApplication loanApplication, string performedBy)
        {
            try
            {
                loanApplication.PerformedBy = performedBy;
                await _loanRepository.UpdateLoanApplicationAsync(loanApplication);
                return (true, "Loan application updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<LoanApplication> GetLoanApplicationByIdAsync(string loanApplicationId)
        {
            return await _loanRepository.GetLoanApplicationByIdAsync(loanApplicationId);
        }

        // Approval
        public async Task<(bool success, string message)> ApproveLoanApplicationAsync(string loanApplicationId, string roleId, string performedBy)
        {
            try
            {
                var tracking = new ApprovalTracking
                {
                    ApprovalTrackingId = Guid.NewGuid().ToString(),
                    LoanApplicationId = loanApplicationId,
                    RoleId = roleId,
                    ApprovalStatus = 1, // Approved
                    ApprovalDate = DateTime.Now,
                    ApprovedBy = performedBy
                };

                await _loanRepository.AddApprovalTrackingAsync(tracking);
                return (true, "Loan application approved.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string message)> RejectLoanApplicationAsync(string loanApplicationId, string roleId, string performedBy)
        {
            try
            {
                var tracking = new ApprovalTracking
                {
                    ApprovalTrackingId = Guid.NewGuid().ToString(),
                    LoanApplicationId = loanApplicationId,
                    RoleId = roleId,
                    ApprovalStatus = -1, // Rejected
                    ApprovalDate = DateTime.Now,
                    ApprovedBy = performedBy
                };

                await _loanRepository.AddApprovalTrackingAsync(tracking);
                return (true, "Loan application rejected.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<ApprovalTracking>> GetApprovalTrackingAsync(string loanApplicationId)
        {
            return await _loanRepository.GetApprovalTrackingByLoanApplicationIdAsync(loanApplicationId);
        }
        public async Task<(bool success, string message)> AddLoanTypeAsync(LoanType loanType, string performedBy)
        {
            try
            {
                loanType.PerformedBy = performedBy; // Set the PerformedBy property
                await _loanRepository.AddLoanTypeAsync(loanType);
                return (true, "Loan type added successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}