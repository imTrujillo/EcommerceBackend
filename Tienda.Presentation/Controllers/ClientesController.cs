using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            return Ok(_clienteRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Cliente cliente)
        {
            return Ok(await _clienteRepository.AddAsync(cliente));
        }

        [HttpGet]
        [Route("cliente/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _clienteRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _clienteRepository.GetByIdAsync(id));
        }

        [HttpDelete]
        [Route("cliente/delete/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _clienteRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _clienteRepository.DeleteAsync(id));
        }

        [HttpPut]
        [Route("cliente/update/{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, Cliente cliente)
        {
            if (id <= 0) return BadRequest();

            if (!await _clienteRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _clienteRepository.UpdateAsync(cliente, id));
        }

    }
}
