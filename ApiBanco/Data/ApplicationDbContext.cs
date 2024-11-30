using ApiBanco.Modelos;
using Microsoft.EntityFrameworkCore;

//3. Creamos el contexto
namespace ApiBanco.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
            {

            }
        //Agregar Modelos

        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Cuenta> cuentas { get; set; }
        public DbSet<Transaccion> transacciones { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación de uno a muchos entre Cuenta y Transaccion (cuentaOrigenId)
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.cuentaOrigen)  // La propiedad de navegación en Transaccion
                .WithMany(c => c.transaccionesOrigen)  // La propiedad de navegación en Cuenta
                .HasForeignKey(t => t.cuentaOrigenId)  // La clave foránea en Transaccion
                .OnDelete(DeleteBehavior.Restrict); // Restricción en caso de eliminación

            // Configurar la relación de uno a muchos entre Cuenta y Transaccion (cuentaDestinoId)
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.cuentaDestino)  // La propiedad de navegación en Transaccion
                .WithMany(c => c.transaccionesDestino)  // La propiedad de navegación en Cuenta
                .HasForeignKey(t => t.cuentaDestinoId)  // La clave foránea en Transaccion
                .OnDelete(DeleteBehavior.Restrict); // Restricción en caso de eliminación
        }
    }
}
