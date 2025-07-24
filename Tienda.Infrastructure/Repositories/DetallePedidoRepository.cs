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
    public class DetallePedidoRepository : BaseRepository<DetallePedido>, IDetallePedidoRepository
    {
        private readonly TiendaDbContext _context;
        public DetallePedidoRepository(TiendaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallePedido
            .Where(d => d.PedidoId == pedidoId)
            .ToListAsync();
        }
    }
}
