using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFApplication.DTOs.Transaction;
using TMFDomain.Entities.Transaction;
using TMFDomain.Interfaces.Account;
using TMFDomain.Interfaces.Transaction;

namespace TMFApplication.Services.Transaction
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }
        public async Task<TransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null) return null;

            return new TransactionDto
            {
                Id = transaction.Id,
                AccountCode = transaction.AccountCode,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Charges = transaction.Charges,
                FinalAmount = transaction.FinalAmount,
                TransactionDate = (DateTime)transaction.created_at,
                Reference = transaction.Reference
            };
        }
        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto, string performedBy)
        {
            var account = await _accountRepository.GetAccountByCodeAsync(dto.AccountCode);
            if (account == null) throw new Exception("Account not found");
            if (!account.IsActive) throw new Exception("Account is not active");

            var charge = await _transactionRepository.GetTransactionChargeAsync(dto.TransactionType, dto.Amount);
            var chargeAmount = CalculateCharge(charge, dto.Amount);
            var finalAmount = dto.TransactionType == "Deposit" ?
                dto.Amount - chargeAmount :
                dto.Amount + chargeAmount;

            if (dto.TransactionType == "Withdrawal" && account.Balance < finalAmount)
                throw new Exception("Insufficient balance");

            account.Balance = dto.TransactionType == "Deposit" ?
                account.Balance + finalAmount :
                account.Balance - finalAmount;

            await _accountRepository.UpdateAccountAsync(account);

            var transaction = new Trans // Fixed: Changed from TransactionService to Transaction
            {
                AccountCode = dto.AccountCode,
                TransactionType = dto.TransactionType,
                Amount = dto.Amount,
                Charges = chargeAmount,
                FinalAmount = finalAmount,
                Reference = dto.Reference ?? GenerateTransactionReference(),
                Description = dto.Description,
                PerformedBy = dto.PerformedBy
            };

            var createdTransaction = await _transactionRepository.CreateTransactionAsync(transaction);

            return new TransactionDto
            {
                Id = createdTransaction.Id,
                AccountCode = createdTransaction.AccountCode,
                TransactionType = createdTransaction.TransactionType,
                Amount = createdTransaction.Amount,
                Charges = createdTransaction.Charges,
                FinalAmount = createdTransaction.FinalAmount,
                TransactionDate = (DateTime)createdTransaction.created_at
            };
        }

        public async Task<IEnumerable<TransactionDto>> GetAccountTransactionsAsync(string accountCode)
        {
            var transactions = await _transactionRepository.GetAccountTransactionsAsync(accountCode);
            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountCode = t.AccountCode,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                Charges = t.Charges,
                FinalAmount = t.FinalAmount,
                TransactionDate = (DateTime)t.created_at,
                Reference = t.Reference
            });
        }

        public async Task<IEnumerable<TransactionChargeDto>> GetAllTransactionChargesAsync()
        {
            var charges = await _transactionRepository.GetAllTransactionChargesAsync();
            return charges.Select(c => new TransactionChargeDto
            {
                Id = c.Id,
                TransactionType = c.TransactionType,
                ChargeType = c.ChargeType,
                Value = c.Value,
                MinAmount = c.MinAmount,
                MaxAmount = c.MaxAmount,
                Description = c.Description,
                IsActive = c.IsActive
            });
        }

        public async Task<TransactionChargeDto> UpdateTransactionChargeAsync(int chargeId, decimal value, bool isActive)
        {
            var charge = await _transactionRepository.GetTransactionChargeByIdAsync(chargeId); // Fixed method call
            if (charge == null) throw new Exception("Transaction charge not found");

            charge.Value = value;
            charge.IsActive = isActive;

            await _transactionRepository.UpdateTransactionChargeAsync(charge);

            return new TransactionChargeDto
            {
                Id = charge.Id,
                TransactionType = charge.TransactionType,
                ChargeType = charge.ChargeType,
                Value = charge.Value,
                IsActive = charge.IsActive
            };
        }

        private decimal CalculateCharge(TransactionCharge charge, decimal amount)
        {
            if (charge == null) return 0;
            return charge.ChargeType == "Fixed" ? charge.Value : amount * (charge.Value / 100);
        }

        private string GenerateTransactionReference()
        {
            return $"TXN-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }
        public async Task<TransactionChargeDto> CreateTransactionChargeAsync(CreateTransactionChargeDto dto)
        {
            if (dto.MaxAmount.HasValue && dto.MaxAmount <= dto.MinAmount)
            {
                throw new Exception("Max amount must be greater than min amount");
            }

            var charge = new TransactionCharge
            {
                TransactionType = dto.TransactionType,
                ChargeType = dto.ChargeType,
                Value = dto.Value,
                MinAmount = dto.MinAmount,
                MaxAmount = dto.MaxAmount,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            var createdCharge = await _transactionRepository.CreateTransactionChargeAsync(charge);

            return new TransactionChargeDto
            {
                Id = createdCharge.Id,
                TransactionType = createdCharge.TransactionType,
                ChargeType = createdCharge.ChargeType,
                Value = createdCharge.Value,
                MinAmount = createdCharge.MinAmount,
                MaxAmount = createdCharge.MaxAmount,
                Description = createdCharge.Description,
                IsActive = createdCharge.IsActive
            };
        }

        public async Task<TransactionChargeDto> UpdateTransactionChargeAsync(int id, UpdateTransactionChargeDto dto)
        {
            var charge = await _transactionRepository.GetTransactionChargeByIdAsync(id);
            if (charge == null)
            {
                throw new Exception("Transaction charge not found");
            }

            charge.Value = dto.Value;
            charge.Description = dto.Description;
            charge.IsActive = dto.IsActive;

            await _transactionRepository.UpdateTransactionChargeAsync(charge);

            return new TransactionChargeDto
            {
                Id = charge.Id,
                TransactionType = charge.TransactionType,
                ChargeType = charge.ChargeType,
                Value = charge.Value,
                MinAmount = charge.MinAmount,
                MaxAmount = charge.MaxAmount,
                Description = charge.Description,
                IsActive = charge.IsActive
            };
        }

        public async Task<TransactionChargeDto> GetTransactionChargeByIdAsync(int id)
        {
            var charge = await _transactionRepository.GetTransactionChargeByIdAsync(id);
            if (charge == null) return null;

            return new TransactionChargeDto
            {
                Id = charge.Id,
                TransactionType = charge.TransactionType,
                ChargeType = charge.ChargeType,
                Value = charge.Value,
                MinAmount = charge.MinAmount,
                MaxAmount = charge.MaxAmount,
                Description = charge.Description,
                IsActive = charge.IsActive
            };
        }
    }
}