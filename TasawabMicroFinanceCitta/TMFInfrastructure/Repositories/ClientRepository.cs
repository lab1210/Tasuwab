using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFDomain.Entities.Client;
using TMFDomain.Entities.Accounts;
using TMFDomain.Interfaces.Client;
using TMFInfrastructure.Data;

namespace TMFInfrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClientInformation> GetClientByIdAsync(int clientId)
        {
            return await _context.ClientInformation
                .Include(c => c.AccountOwners)
                .ThenInclude(o => o.Account)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<IEnumerable<ClientInformation>> GetAllClientsAsync()
        {
            return await _context.ClientInformation.ToListAsync();
        }

        public async Task<ClientInformation> CreateClientAsync(ClientInformation client)
        {
            _context.ClientInformation.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task UpdateClientAsync(ClientInformation client)
        {
            _context.ClientInformation.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountEntity>> GetClientAccountsAsync(int clientId)
        {
            return await _context.AccountOwners
                .Where(o => o.ClientId == clientId)
                .Include(o => o.Account)
                .ThenInclude(a => a.AccountType)
                .Select(o => o.Account)
                .ToListAsync();
        }
    }
}
