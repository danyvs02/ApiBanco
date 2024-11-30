using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class ActualizarCuentaDto
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int usuarioId { get; set; }

        [Required(ErrorMessage = "El numero de cuenta es obligatoria")]
        [StringLength(20)]
        public string numeroCuenta { get; set; }

        
        [Range(0, double.MaxValue)]
        public decimal saldo { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es obligatoria")]
        [StringLength(50)]
        public string tipoCuenta { get; set; }

        public string estado { get; set; }

 


     
    }
}
