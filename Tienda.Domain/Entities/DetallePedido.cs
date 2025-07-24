using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Domain.Entities
{
    public class DetallePedido : BaseEntity
    {
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
