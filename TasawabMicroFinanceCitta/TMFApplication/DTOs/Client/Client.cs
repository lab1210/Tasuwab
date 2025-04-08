using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFApplication.DTOs.Client
{
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DateOfBirth { get; set; }
        public bool Status { get; set; }
        public List<string> AccountCodes { get; set; } = new();
    }
    public class CreateClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public int DateOfBirth { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string BranchCode { get; set; }
        public string PerformedBy { get; set; }
    }
    public class UpdateClientDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string MartialStatus { get; set; }
        public bool Status { get; set; }
    }
}
