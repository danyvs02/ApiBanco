using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class CuentaCrearDto
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int usuarioId { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal saldo { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es obligatoria")]
        [StringLength(50)]
        public string tipoCuenta { get; set; } = "Debito";

        public string estado { get; set; } = "Activo";

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime fechaCreacion { get; set; } = DateTime.Now;

        // Nuevo campo para la clave
        [Required(ErrorMessage = "La clave es obligatoria")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "La clave debe ser de 4 dígitos")]
        public string clave { get; set; }
    }
}
