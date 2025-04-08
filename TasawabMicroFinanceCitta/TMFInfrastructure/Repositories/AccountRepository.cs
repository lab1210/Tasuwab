using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Accounts;
using TMFDomain.Interfaces.Account;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccountEntity> GetAccountByCodeAsync(string accountCode)
        {
            return await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.AccountEntityType)
                .Include(a => a.AccountOwners)
                .ThenInclude(o => o.Client)
                .FirstOrDefaultAsync(a => a.AccountCode == accountCode);
        }

        public async Task<IEnumerable<AccountEntity>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.AccountEntityType)
                .ToListAsync();
        }

        public async Task<AccountEntity> CreateAccountAsync(AccountEntity account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task UpdateAccountAsync(AccountEntity account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AccountExistsAsync(string accountCode)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountCode == accountCode);
        }

        public async Task<AccountType> GetAccountTypeByCodeAsync(string accountTypeCode)
        {
            return await _context.AccountTypes.FindAsync(accountTypeCode);
        }

        public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync()
        {
            return await _context.AccountTypes.ToListAsync();
        }

        public async Task UpdateAccountTypeAsync(AccountType accountType)
        {
            _context.AccountTypes.Update(accountType);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountEntityType> GetAccountEntityTypeByCodeAsync(string entityTypeCode)
        {
            return await _context.AccountEntityTypes.FindAsync(entityTypeCode);
        }

        public async Task<IEnumerable<AccountEntityType>> GetAllAccountEntityTypesAsync()
        {
            return await _context.AccountEntityTypes.ToListAsync();
        }

        public async Task UpdateAccountEntityTypeAsync(AccountEntityType entityType)
        {
            _context.AccountEntityTypes.Update(entityType);
            await _context.SaveChangesAsync();
        }

        public async Task AddAccountOwnerAsync(AccountOwner owner)
        {
            _context.AccountOwners.Add(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountOwner>> GetAccountOwnersAsync(string accountCode)
        {
            return await _context.AccountOwners
                .Include(o => o.Client)
                .Where(o => o.AccountCode == accountCode)
                .ToListAsync();
        }
        public async Task CreateAccountTypeAsync(AccountType accountType)
        {
            _context.AccountTypes.Add(accountType);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAccountEntityTypeAsync(AccountEntityType entityType)
        {
            _context.AccountEntityTypes.Add(entityType);
            await _context.SaveChangesAsync();
        }
    }
}
