using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Domain.Entities
{
    public class Pedido : BaseEntity
    {
        public Pedido()
        {
            Detalles = new List<DetallePedido>();
        }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string MetodoPago { get; set; }
        public string DireccionEnvio { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; }
        
    }
}
