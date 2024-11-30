using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos
{
    public class Cuenta
    {
        [Key]
        public int id { get; set; }

        [Required]  
        public int usuarioId { get; set; }

        [Required] 
        [StringLength(20)]  
        public string numeroCuenta { get; set; }

        [Required]
        [StringLength(32)] // Para almacenar la clave encriptada (MD5 tiene 32 caracteres)
        public string clave { get; set; }

        [Range(0, double.MaxValue)]  
        public decimal saldo { get; set; }

        [Required]
        [StringLength(50)]
        [DefaultValue("Debito")]
        public string tipoCuenta { get; set; } = "Debito";
        [DefaultValue("Activo")]
        public string estado { get; set; } = "Activo";

        [Required]  
        public DateTime fechaCreacion { get; set; } = DateTime.Now;


        public Usuario usuario { get; set; }

        public ICollection<Transaccion> transaccionesOrigen { get; set; }  
        public ICollection<Transaccion> transaccionesDestino { get; set; }  
    }
}
