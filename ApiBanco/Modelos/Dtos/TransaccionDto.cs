using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class TransaccionDto
    {
 
        public int id { get; set; }

        [Required(ErrorMessage = "El número de cuenta de origen es obligatorio")]
        public string numeroCuentaOrigen { get; set; }

        [Required(ErrorMessage = "El número de cuenta de destino es obligatorio")]
        public string numeroCuentaDestino { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        public decimal monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime fechaTransaccion { get; set; } = DateTime.Now;

        // También puedes agregar tipoTransaccion para visualizarlo si es necesario
        public string tipoTransaccion { get; set; }

        public string estado { get; set; }

        public string descripcion { get; set; }

        // Campo nuevo para la clave
        public string clave { get; set; }
    }
}
