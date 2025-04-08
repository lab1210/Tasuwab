using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMFApplication.DTOs.Account;
using TMFApplication.DTOs.Client;
using TMFApplication.Services.Client;

namespace TasawabMicroFinanceCitta.Controllers.Client
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [Authorize(Policy = "CreateClients")]
        [HttpPost]
        public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto dto)
        {
            try
            {
                var client = await _clientService.CreateClientAsync(dto);
                return CreatedAtAction(nameof(GetClient), new { clientId = client.ClientId }, client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ViewClients")]
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<ClientDto>> GetClient(int clientId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client == null) return NotFound();
            return Ok(client);
        }
        [Authorize(Policy = "ViewClients")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }
        [Authorize(Policy = "UpdateClients")]
        [HttpPatch("update/{clientId}")]
        public async Task<ActionResult<ClientDto>> UpdateClient(int clientId, UpdateClientDto dto)
        {
            try
            {
                var client = await _clientService.UpdateClientAsync(clientId, dto);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = "ViewClients")]
        [HttpGet("{clientId}/accounts")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetClientAccounts(int clientId)
        {
            var accounts = await _clientService.GetClientAccountsAsync(clientId);
            return Ok(accounts);
        }

    }
}