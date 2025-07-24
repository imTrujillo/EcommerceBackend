using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Domain.Entities;

namespace Tienda.Domain.Interfaces
{
    public interface IDetallePedidoRepository : IBaseRepository<DetallePedido>
    {
        Task<IEnumerable<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
    }
}
