using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;

namespace Tienda.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var productos = _productoRepository.GetAll();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (producto == null) return BadRequest();

            var result = await _productoRepository.AddAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _productoRepository.GetAnyAsync(id))
                return NotFound();

            var producto = await _productoRepository.GetByIdAsync(id);
            return Ok(producto);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _productoRepository.GetAnyAsync(id))
                return NotFound();

            var result = await _productoRepository.DeleteAsync(id);
            return Ok(new { success = result, message = "Producto eliminado" });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, Producto producto)
        {
            if (id <= 0 || producto == null)
                return BadRequest();

            if (id != producto.Id)
                return BadRequest("ID del producto no coincide");

            if (!await _productoRepository.GetAnyAsync(id))
                return NotFound();

            var result = await _productoRepository.UpdateAsync(producto, id);
            return Ok(result);
        }
    }
}
