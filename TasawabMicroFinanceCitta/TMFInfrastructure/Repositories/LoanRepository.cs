using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDomain.Entities.Loan;
using TMFDomain.Interfaces.Loan;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class LoanRepository : Repository<LoanApplication>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Loan Type
        public async Task AddLoanTypeAsync(LoanType loanType)
        {
            await _context.LoanTypes.AddAsync(loanType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLoanTypeAsync(LoanType loanType)
        {
            _context.LoanTypes.Update(loanType);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoanType>> GetAllLoanTypesAsync()
        {
            return await _context.LoanTypes.ToListAsync();
        }

        // Loan Application
        public async Task AddLoanApplicationAsync(LoanApplication loanApplication)
        {
            await _context.LoanApplications.AddAsync(loanApplication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLoanApplicationAsync(LoanApplication loanApplication)
        {
            _context.LoanApplications.Update(loanApplication);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanApplication> GetLoanApplicationByIdAsync(string loanApplicationId)
        {
            return await _context.LoanApplications
                .FirstOrDefaultAsync(la => la.LoanApplicationId == loanApplicationId);
        }

        // Approval
        public async Task AddApprovalTrackingAsync(ApprovalTracking tracking)
        {
            await _context.ApprovalTrackings.AddAsync(tracking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalTracking>> GetApprovalTrackingByLoanApplicationIdAsync(string loanApplicationId)
        {
            return await _context.ApprovalTrackings
                .Where(at => at.LoanApplicationId == loanApplicationId)
                .ToListAsync();
        }
    }
}