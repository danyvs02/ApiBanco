using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class TransaccionDepositarDto
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El número de cuenta destino es obligatorio")]
        public string numeroCuentaDestino { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        public decimal monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime fechaTransaccion { get; set; } = DateTime.Now;

        public string estado { get; set; }

        public string descripcion { get; set; }

 
    }
}
