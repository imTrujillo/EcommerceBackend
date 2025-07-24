using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public IActionResult GetCategorias()
        {
            return Ok(_categoriaRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoria(Categoria categoria)
        {
            return Ok(await _categoriaRepository.AddAsync(categoria));
        }

        [HttpGet]
        [Route("categoria/{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _categoriaRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _categoriaRepository.GetByIdAsync(id));
        }

        [HttpDelete]
        [Route("categoria/delete/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _categoriaRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _categoriaRepository.DeleteAsync(id));
        }

        [HttpPut]
        [Route("categoria/update/{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, Categoria categoria)
        {
            if (id <= 0) return BadRequest();
            if (!await _categoriaRepository.GetAnyAsync(id)) return NotFound();

            return Ok(await _categoriaRepository.UpdateAsync(categoria, id));
        }
    }
}
