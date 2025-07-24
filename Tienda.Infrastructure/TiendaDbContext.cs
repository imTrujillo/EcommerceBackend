using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Domain.Entities;

namespace Tienda.Infrastructure
{
    public class TiendaDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente>  Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedido { get; set; }
        public TiendaDbContext(DbContextOptions<TiendaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Primary Keys
            modelBuilder.Entity<Producto>().HasKey(p => p.Id);
            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.Id);
            modelBuilder.Entity<Categoria>().HasKey(c => c.Id);
            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
            modelBuilder.Entity<DetallePedido>().HasKey(d => d.Id);

            //Foreign Keys
            modelBuilder.Entity<Producto>().HasOne(p => p.Categoria).WithMany(c => c.Productos).HasForeignKey(p => p.CategoriaId);
            modelBuilder.Entity<Producto>().HasOne(p => p.Proveedor).WithMany(p => p.Productos).HasForeignKey(p => p.ProveedorId);
            modelBuilder.Entity<Pedido>().HasOne(p => p.Cliente).WithMany(c => c.Pedidos).HasForeignKey(p => p.ClienteId);
            modelBuilder.Entity<DetallePedido>().HasOne(d => d.Pedido).WithMany(p => p.Detalles).HasForeignKey(d => d.PedidoId);
            modelBuilder.Entity<DetallePedido>().HasOne(d => d.Producto).WithMany(p => p.Detalles).HasForeignKey(d => d.ProductoId);
        }
    }
}
