using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Transaction;
using TMFDomain.Interfaces.Transaction;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionCharge> GetTransactionChargeByIdAsync(int id)
        {
            return await _context.TransactionCharges.FindAsync(id);
        }

        public async Task<IEnumerable<TransactionCharge>> GetAllTransactionChargesAsync()
        {
            return await _context.TransactionCharges.ToListAsync();
        }

        public async Task UpdateTransactionChargeAsync(TransactionCharge charge)
        {
            _context.TransactionCharges.Update(charge);
            await _context.SaveChangesAsync();
        }
        public async Task<Trans> CreateTransactionAsync(Trans transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Trans>> GetAccountTransactionsAsync(string accountCode)
        {
            return await _context.Transactions
                .Where(t => t.AccountCode == accountCode)
                .OrderByDescending(t => t.created_at)
                .ToListAsync();
        }

        public async Task<Trans> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<TransactionCharge> GetTransactionChargeAsync(string transactionType, decimal amount)
        {
            return await _context.TransactionCharges
                .Where(tc => tc.TransactionType == transactionType &&
                             tc.IsActive &&
                             amount >= tc.MinAmount &&
                             (tc.MaxAmount == null || amount <= tc.MaxAmount))
                .OrderBy(tc => tc.MinAmount)
                .FirstOrDefaultAsync();
        }
        public async Task<TransactionCharge> CreateTransactionChargeAsync(TransactionCharge charge)
        {
            _context.TransactionCharges.Add(charge);
            await _context.SaveChangesAsync();
            return charge;
        }


    }
}
