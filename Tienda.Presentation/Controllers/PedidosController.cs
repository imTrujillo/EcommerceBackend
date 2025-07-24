using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepository _pedidosRepository;
        private readonly IDetallePedidoRepository _detallesRepository;
        private readonly IProductoRepository _productosRepository;

        public PedidosController(IPedidoRepository pedidosRepository, IDetallePedidoRepository detallesRepository, IProductoRepository productoRepository)
        {
            _pedidosRepository = pedidosRepository;
            _detallesRepository = detallesRepository;
            _productosRepository = productoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = _pedidosRepository.GetAll().ToList();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _pedidosRepository.GetAnyAsync(id))
                return NotFound();

            var pedido = await _pedidosRepository.GetByIdAsync(id);
            return Ok(pedido);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            if (!await _pedidosRepository.GetAnyAsync(id))
                return NotFound();

            // Eliminar detalles primero
            var detalles = _detallesRepository.GetAll()
                    .Where(d => d.PedidoId == id)
                    .ToList();

            foreach (var detalle in detalles)
            {
                await _detallesRepository.DeleteAsync(detalle.Id);
            }

            var result = await _pedidosRepository.DeleteAsync(id);
            return Ok(new { success = result, message = "Pedido eliminado" });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, Pedido pedido)
        {
            if (id <= 0 || pedido == null)
                return BadRequest();

            if (id != pedido.Id)
                return BadRequest("ID del pedido no coincide");

            if (!await _pedidosRepository.GetAnyAsync(id))
                return NotFound();

            var result = await _pedidosRepository.UpdateAsync(pedido, id);
            return Ok(result);
        }

        // Métodos específicos para manejo de detalles
        [HttpPost("crear-con-detalles")]
        public async Task<IActionResult> CreateWithDetails(PedidoConDetallesRequest request)
        {
            if (request?.Detalles == null || !request.Detalles.Any())
                return BadRequest("Debe incluir detalles del pedido");

            // Crear pedido principal
            var pedido = new Pedido
            {
                ClienteId = request.ClienteID,
                Fecha = request.Fecha,
                Estado = "Pendiente",
                MetodoPago = request.MetodoPago,
                DireccionEnvio = request.DireccionEnvio
            };

            await _pedidosRepository.AddAsync(pedido);

            // Crear detalles
            foreach (var detalle in request.Detalles)
            {
                // Validar existencia de producto
                if (!await _productosRepository.GetAnyAsync(detalle.ProductoID))
                    return NotFound($"Producto con ID {detalle.ProductoID} no encontrado");

                var nuevoDetalle = new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = detalle.ProductoID,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                };

                await _detallesRepository.AddAsync(nuevoDetalle);
            }

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }

        [HttpGet("{pedidoId}/detalles")]
        public async Task<IActionResult> GetDetallesByPedido(int pedidoId)
        {
            if (pedidoId <= 0) return BadRequest();

            if (!await _pedidosRepository.GetAnyAsync(pedidoId))
                return NotFound("Pedido no encontrado");

            var detalles = _detallesRepository.GetAll()
                    .Where(d => d.PedidoId == pedidoId)
                    .ToList();

            return Ok(detalles);
        }

        [HttpPost("agregar-detalle/{pedidoId}")]
        public async Task<IActionResult> AddDetalle(int pedidoId, DetallePedidoRequest detalle)
        {
            if (pedidoId <= 0 || detalle == null)
                return BadRequest();

            if (!await _pedidosRepository.GetAnyAsync(pedidoId))
                return NotFound("Pedido no encontrado");

            if (!await _productosRepository.GetAnyAsync(detalle.ProductoID))
                return NotFound($"Producto con ID {detalle.ProductoID} no encontrado");

            var nuevoDetalle = new DetallePedido
            {
                PedidoId = pedidoId,
                ProductoId = detalle.ProductoID,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario
            };

            var result = await _detallesRepository.AddAsync(nuevoDetalle);
            return Ok(result);
        }

        [HttpDelete("eliminar-detalle/{detalleId}")]
        public async Task<IActionResult> DeleteDetalle(int detalleId)
        {
            if (detalleId <= 0) return BadRequest();

            if (!await _detallesRepository.GetAnyAsync(detalleId))
                return NotFound();

            var result = await _detallesRepository.DeleteAsync(detalleId);
            return Ok(new { success = result, message = "Detalle eliminado" });
        }
    }

    // Clases de apoyo
    public class PedidoConDetallesRequest
    {
        public int ClienteID { get; set; }
        public string MetodoPago { get; set; }
        public string DireccionEnvio { get; set; }
        public List<DetallePedidoRequest> Detalles { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class DetallePedidoRequest
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
    }
}
