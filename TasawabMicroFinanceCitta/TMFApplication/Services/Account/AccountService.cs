using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Account;
using TMFApplication.DTOs.Client;
using TMFApplication.DTOs.Transaction;
using TMFDomain.Entities.Accounts;
using TMFDomain.Entities.Transaction;
using TMFDomain.Interfaces.Account;
using TMFDomain.Interfaces.Client;
using TMFDomain.Interfaces.Transaction;

namespace TMFApplication.Services.Account
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;

        public AccountService(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        public async Task<AccountDto> GetAccountByCodeAsync(string accountCode)
        {
            var account = await _accountRepository.GetAccountByCodeAsync(accountCode);
            if (account == null) return null;

            return new AccountDto
            {
                AccountCode = account.AccountCode,
                AccountTypeCode = account.AccountTypeCode,
                EntityTypeCode = account.EntityTypeCode,
                Balance = account.Balance,
                IsActive = account.IsActive,
                BranchCode = account.BranchCode,
                Owners = account.AccountOwners.Select(o => new AccountOwnerDto
                {
                    ClientId = o.ClientId,
                    OwnershipType = o.OwnershipType,
                    OwnershipPercentage = o.OwnershipPercentage
                }).ToList()
            };
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            return accounts.Select(a => new AccountDto
            {
                AccountCode = a.AccountCode,
                AccountTypeCode = a.AccountTypeCode,
                EntityTypeCode = a.EntityTypeCode,
                Balance = a.Balance,
                IsActive = a.IsActive,
                BranchCode = a.BranchCode
            });
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
        {
            // Generate account code (implement your own logic)
            var accountCode = GenerateAccountCode(dto.AccountTypeCode);

            var account = new AccountEntity
            {
                AccountCode = accountCode,
                AccountTypeCode = dto.AccountTypeCode,
                EntityTypeCode = dto.EntityTypeCode,
                Balance = 0,
                IsActive = true,
                BranchCode = dto.BranchCode,
                OpenDate = DateTime.Now
            };

            // Validate entity type and owners count
            var entityType = await _accountRepository.GetAccountEntityTypeByCodeAsync(dto.EntityTypeCode);
            if (entityType == null) throw new Exception("Invalid entity type");

            if (dto.Owners.Count < entityType.MinOwners || dto.Owners.Count > entityType.MaxOwners)
                throw new Exception($"Number of owners must be between {entityType.MinOwners} and {entityType.MaxOwners}");

            // Create account
            var createdAccount = await _accountRepository.CreateAccountAsync(account);

            // Add owners
            foreach (var ownerDto in dto.Owners)
            {
                var client = await _clientRepository.GetClientByIdAsync(ownerDto.ClientId);
                if (client == null) throw new Exception($"Client with ID {ownerDto.ClientId} not found");

                var owner = new AccountOwner
                {
                    AccountCode = accountCode,
                    ClientId = ownerDto.ClientId,
                    OwnershipType = ownerDto.OwnershipType,
                    OwnershipPercentage = ownerDto.OwnershipPercentage
                };

                await _accountRepository.AddAccountOwnerAsync(owner);
            }

            return await GetAccountByCodeAsync(accountCode);
        }

        public async Task<AccountDto> UpdateAccountStatusAsync(string accountCode, bool isActive)
        {
            var account = await _accountRepository.GetAccountByCodeAsync(accountCode);
            if (account == null) throw new Exception("Account not found");

            account.IsActive = isActive;
            await _accountRepository.UpdateAccountAsync(account);

            return await GetAccountByCodeAsync(accountCode);
        }

        public async Task<IEnumerable<AccountTypeDto>> GetAllAccountTypesAsync()
        {
            var types = await _accountRepository.GetAllAccountTypesAsync();
            return types.Select(t => new AccountTypeDto
            {
                AccountTypeCode = t.AccountTypeCode,
                Name = t.Name,
                InterestRate = t.InterestRate,
                Description = t.Description,
                IsActive = t.IsActive
            });
        }

        public async Task<AccountTypeDto> UpdateAccountTypeInterestRateAsync(string accountTypeCode, decimal interestRate)
        {
            var type = await _accountRepository.GetAccountTypeByCodeAsync(accountTypeCode);
            if (type == null) throw new Exception("Account type not found");

            type.InterestRate = interestRate;
            await _accountRepository.UpdateAccountTypeAsync(type);

            return new AccountTypeDto
            {
                AccountTypeCode = type.AccountTypeCode,
                Name = type.Name,
                InterestRate = type.InterestRate,
                Description = type.Description,
                IsActive = type.IsActive
            };
        }

        public async Task<IEnumerable<AccountEntityTypeDto>> GetAllAccountEntityTypesAsync()
        {
            var types = await _accountRepository.GetAllAccountEntityTypesAsync();
            return types.Select(t => new AccountEntityTypeDto
            {
                EntityTypeCode = t.EntityTypeCode,
                Name = t.Name,
                MinOwners = t.MinOwners,
                MaxOwners = t.MaxOwners,
                Description = t.Description,
                IsActive = t.IsActive
            });
        }

        public async Task<AccountEntityTypeDto> UpdateAccountEntityTypeAsync(string entityTypeCode, int minOwners, int maxOwners)
        {
            var type = await _accountRepository.GetAccountEntityTypeByCodeAsync(entityTypeCode);
            if (type == null) throw new Exception("Entity type not found");

            type.MinOwners = minOwners;
            type.MaxOwners = maxOwners;
            await _accountRepository.UpdateAccountEntityTypeAsync(type);

            return new AccountEntityTypeDto
            {
                EntityTypeCode = type.EntityTypeCode,
                Name = type.Name,
                MinOwners = type.MinOwners,
                MaxOwners = type.MaxOwners,
                Description = type.Description,
                IsActive = type.IsActive
            };
        }

        private string GenerateAccountCode(string accountTypeCode)
        {
            // Implement your account code generation logic
            // Example: "SAV-20230501-0001"
            return $"{accountTypeCode}-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }

        public async Task<AccountTypeDto> CreateAccountTypeAsync(CreateAccountTypeDto dto)
        {
            var existingType = await _accountRepository.GetAccountTypeByCodeAsync(dto.AccountTypeCode);
            if (existingType != null) throw new Exception("Account type with this code already exists");

            var accountType = new AccountType
            {
                AccountTypeCode = dto.AccountTypeCode,
                Name = dto.Name,
                InterestRate = dto.InterestRate,
                Description = dto.Description,
                IsActive = true
            };

            await _accountRepository.CreateAccountTypeAsync(accountType);

            return new AccountTypeDto
            {
                AccountTypeCode = accountType.AccountTypeCode,
                Name = accountType.Name,
                InterestRate = accountType.InterestRate,
                Description = accountType.Description,
                IsActive = accountType.IsActive
            };
        }

        public async Task<AccountEntityTypeDto> CreateAccountEntityTypeAsync(CreateAccountEntityTypeDto dto)
        {
            var existingType = await _accountRepository.GetAccountEntityTypeByCodeAsync(dto.EntityTypeCode);
            if (existingType != null) throw new Exception("Entity type with this code already exists");

            if (dto.MinOwners < 1) throw new Exception("Minimum owners must be at least 1");
            if (dto.MaxOwners < dto.MinOwners) throw new Exception("Maximum owners cannot be less than minimum owners");

            var entityType = new AccountEntityType
            {
                EntityTypeCode = dto.EntityTypeCode,
                Name = dto.Name,
                MinOwners = dto.MinOwners,
                MaxOwners = dto.MaxOwners,
                Description = dto.Description,
                IsActive = true
            };

            await _accountRepository.CreateAccountEntityTypeAsync(entityType);

            return new AccountEntityTypeDto
            {
                EntityTypeCode = entityType.EntityTypeCode,
                Name = entityType.Name,
                MinOwners = entityType.MinOwners,
                MaxOwners = entityType.MaxOwners,
                Description = entityType.Description,
                IsActive = entityType.IsActive
            };
        }

        public async Task<AccountTypeDto> UpdateAccountTypeAsync(string accountTypeCode, UpdateAccountTypeDto dto)
        {
            var type = await _accountRepository.GetAccountTypeByCodeAsync(accountTypeCode);
            if (type == null) throw new Exception("Account type not found");

            type.Name = dto.Name;
            type.InterestRate = dto.InterestRate;
            type.Description = dto.Description;
            type.IsActive = dto.IsActive;

            await _accountRepository.UpdateAccountTypeAsync(type);

            return new AccountTypeDto
            {
                AccountTypeCode = type.AccountTypeCode,
                Name = type.Name,
                InterestRate = type.InterestRate,
                Description = type.Description,
                IsActive = type.IsActive
            };
        }
    }
}

