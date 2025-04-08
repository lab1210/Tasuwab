using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Transaction;

namespace TMFDomain.Interfaces.Transaction
{
    public interface ITransactionRepository
    {
        Task<Trans> CreateTransactionAsync(Trans transaction);
        Task<IEnumerable<Trans>> GetAccountTransactionsAsync(string accountCode);
        Task<Trans> GetTransactionByIdAsync(int id);
        Task<TransactionCharge> GetTransactionChargeAsync(string transactionType, decimal amount);
        Task<TransactionCharge> CreateTransactionChargeAsync(TransactionCharge charge);
        Task<TransactionCharge> GetTransactionChargeByIdAsync(int id);
        Task<IEnumerable<TransactionCharge>> GetAllTransactionChargesAsync();
        Task UpdateTransactionChargeAsync(TransactionCharge charge);
    }
}
