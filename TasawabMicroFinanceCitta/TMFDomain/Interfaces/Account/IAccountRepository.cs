using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Accounts;

namespace TMFDomain.Interfaces.Account
{
    public interface IAccountRepository
    {
        Task<AccountEntity> GetAccountByCodeAsync(string accountCode);
        Task<IEnumerable<AccountEntity>> GetAllAccountsAsync();
        Task<AccountEntity> CreateAccountAsync(AccountEntity account);
        Task UpdateAccountAsync(AccountEntity account);
        Task<bool> AccountExistsAsync(string accountCode);

        Task<AccountType> GetAccountTypeByCodeAsync(string accountTypeCode);
        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync();
        Task UpdateAccountTypeAsync(AccountType accountType);

        Task<AccountEntityType> GetAccountEntityTypeByCodeAsync(string entityTypeCode);
        Task<IEnumerable<AccountEntityType>> GetAllAccountEntityTypesAsync();
        Task UpdateAccountEntityTypeAsync(AccountEntityType entityType);

        Task AddAccountOwnerAsync(AccountOwner owner);
        Task<IEnumerable<AccountOwner>> GetAccountOwnersAsync(string accountCode);
        Task CreateAccountTypeAsync(AccountType accountType);
        Task CreateAccountEntityTypeAsync(AccountEntityType entityType);

    }
}
