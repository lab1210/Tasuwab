using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Accounts;
using TMFDomain.Entities.Client;

namespace TMFDomain.Interfaces.Client
{
    public interface IClientRepository
    {
        Task<ClientInformation> GetClientByIdAsync(int clientId);
        Task<IEnumerable<ClientInformation>> GetAllClientsAsync();
        Task<ClientInformation> CreateClientAsync(ClientInformation client);
        Task UpdateClientAsync(ClientInformation client);
        Task<IEnumerable<AccountEntity>> GetClientAccountsAsync(int clientId);
    }
}
