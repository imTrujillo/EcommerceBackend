using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedoresController(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        [HttpGet]
        public IActionResult GetProveedores()
        {
            return Ok (_proveedorRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> AddProveedor(Proveedor proveedor)
        {
            return Ok(await _proveedorRepository.AddAsync(proveedor));
        }

        [HttpGet]
        [Route("proveedor/{id}")]
        public async Task<IActionResult> GetProveedorById(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _proveedorRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _proveedorRepository.GetByIdAsync(id));
        }

        [HttpDelete]
        [Route("proveedor/delete/{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _proveedorRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _proveedorRepository.DeleteAsync(id));
        }

        [HttpPut]
        [Route("proveedor/update/{id}")]
        public async Task<IActionResult> UpdateProveedor(int id, Proveedor proveedor)
        {
            if (id <= 0) return BadRequest();
            if (!await _proveedorRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _proveedorRepository.UpdateAsync(proveedor, id));
        }
    }
}
