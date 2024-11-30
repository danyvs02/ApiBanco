using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos
{
    public class Transaccion
    {
        [Key]
        public int id { get; set; }

        [Required] 
        public int cuentaOrigenId { get; set; }

        [Required]  
        public int cuentaDestinoId { get; set; }

        [Required]  
        public string tipoTransaccion { get; set; }

        [Required]  
        public decimal monto { get; set; }

        [Required] 
        public DateTime fechaTransaccion { get; set; } = DateTime.Now;

        public string estado { get; set; }           

        public string descripcion { get; set; }

        // Nueva propiedad para la clave
       
        [StringLength(32)]  // Longitud de la clave puede ser configurada según lo que consideres adecuado
        public string clave { get; set; }

        public Cuenta cuentaOrigen { get; set; }
        public Cuenta cuentaDestino { get; set; }
    }
}
