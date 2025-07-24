using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Domain.Entities;
using Tienda.Domain.Interfaces;

namespace Tienda.Infrastructure.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly TiendaDbContext _context;
        public PedidoRepository(TiendaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pedido> CreateWithDetailsAsync(Pedido pedido, List<DetallePedido> detalles)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();

            // Asignar ID del pedido a los detalles
            foreach (var detalle in detalles)
            {
                detalle.PedidoId = pedido.Id;
                await _context.DetallePedido.AddAsync(detalle);
            }

            await _context.SaveChangesAsync();
            return pedido;
        }
    }
}
