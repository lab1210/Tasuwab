using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFApplication.DTOs.Account;
using TMFApplication.DTOs.Client;
using TMFDomain.Interfaces.Client;
using TMFDomain.Entities.Client;


namespace TMFApplication.Services.Client
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> GetClientByIdAsync(int clientId)
        {
            var client = await _clientRepository.GetClientByIdAsync(clientId);
            if (client == null) return null;

            return new ClientDto
            {
                ClientId = client.ClientId,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                DateOfBirth = client.DateOfBirth,
                Status = client.Status,
                AccountCodes = client.AccountOwners.Select(o => o.AccountCode).ToList()
            };
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            return clients.Select(c => new ClientDto
            {
                ClientId = c.ClientId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                DateOfBirth = c.DateOfBirth,
                Status = c.Status
            });
        }
        public async Task<ClientDto> CreateClientAsync(CreateClientDto dto)
        {
            var client = new ClientInformation
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Gender = dto.Gender,
                MartialStatus = dto.MartialStatus,
                DateOfBirth = dto.DateOfBirth,
                IdentificationType = dto.IdentificationType,
                IdentificationNumber = dto.IdentificationNumber,
                Status = true,
                BranchCode = dto.BranchCode,
                PerformedBy = dto.PerformedBy  // Use the provided value
            };

            var createdClient = await _clientRepository.CreateClientAsync(client);
            return await GetClientByIdAsync(createdClient.ClientId);
        }

        public async Task<ClientDto> UpdateClientAsync(int clientId, UpdateClientDto dto)
        {
            var client = await _clientRepository.GetClientByIdAsync(clientId);
            if (client == null) throw new Exception("Client not found");

            client.Email = dto.Email ?? client.Email;
            client.PhoneNumber = dto.PhoneNumber ?? client.PhoneNumber;
            client.Address = dto.Address ?? client.Address;
            client.MartialStatus = dto.MartialStatus ?? client.MartialStatus;
            client.Status = dto.Status;

            await _clientRepository.UpdateClientAsync(client);

            return await GetClientByIdAsync(clientId);
        }

        public async Task<IEnumerable<AccountDto>> GetClientAccountsAsync(int clientId)
        {
            var accounts = await _clientRepository.GetClientAccountsAsync(clientId);
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
    }
}
