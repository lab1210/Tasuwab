using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFApplication.DTOs.Account
{
    public class AccountTypeDto
    {
        public string AccountTypeCode { get; set; }
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class AccountEntityTypeDto
    {
        public string EntityTypeCode { get; set; }
        public string Name { get; set; }
        public int MinOwners { get; set; }
        public int MaxOwners { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccountDto
    {
        public string AccountCode { get; set; }
        public string AccountTypeCode { get; set; }
        public string EntityTypeCode { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public string BranchCode { get; set; }
        public List<AccountOwnerDto> Owners { get; set; } = new();
    }

    public class AccountOwnerDto
    {
        public int ClientId { get; set; }
        public string OwnershipType { get; set; }
        public decimal OwnershipPercentage { get; set; }
    }

    public class CreateAccountDto
    {
        public string AccountTypeCode { get; set; }
        public string EntityTypeCode { get; set; }
        public string BranchCode { get; set; }
        public List<AccountOwnerDto> Owners { get; set; } = new();
    }

    public class UpdateAccountDto
    {
        public bool IsActive { get; set; }
    }
    public class UpdateEntityTypeDto
    {
        public int MinOwners { get; set; }
        public int MaxOwners { get; set; }
    }
    public class CreateAccountTypeDto
    {
        public string AccountTypeCode { get; set; }
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public string Description { get; set; }
    }

    public class CreateAccountEntityTypeDto
    {
        public string EntityTypeCode { get; set; }
        public string Name { get; set; }
        public int MinOwners { get; set; }
        public int MaxOwners { get; set; }
        public string Description { get; set; }
    }

    public class UpdateAccountTypeDto
    {
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
