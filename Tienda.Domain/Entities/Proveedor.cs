using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Domain.Entities
{
    public class Proveedor : BaseEntity
    {
        public Proveedor() 
        {
            Productos = new List<Producto>();
        }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}
