using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Accounts;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Client
{
    public class ClientInformation : BaseEntity
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int DateOfBirth { get; set; } // Format: YYYYMMDD
        public string? IdentificationType { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? ClientImage { get; set; }
        public string? ClientCreationForm { get; set; }
        public bool Status { get; set; } = true;
        public string? BranchCode { get; set; }
        public string? PerformedBy { get; set; } = string.Empty;

        // Navigation property
        public ICollection<AccountOwner> AccountOwners { get; set; }
    }
}
