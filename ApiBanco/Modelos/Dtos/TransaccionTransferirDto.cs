using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class TransaccionTransferirDto
    {
        public int id { get; set; }

        [Required(ErrorMessage = "La cuenta de origen es obligatoria")]
        public string numeroCuentaOrigen { get; set; }
        [Required(ErrorMessage = "La cuenta de destino es obligatoria")]
        public string numeroCuentaDestino { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        public decimal monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime fechaTransaccion { get; set; } = DateTime.Now;

        public string estado { get; set; }

        public string descripcion { get; set; }

        // Nueva propiedad para la clave
        [Required(ErrorMessage = "La clave es obligatoria")]
        public string clave { get; set; }
    }
}
