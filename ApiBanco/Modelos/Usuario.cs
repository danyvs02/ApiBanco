using ApiBanco.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiBanco.Modelos
{
    public class Usuario
    {
        [Key]
        public int id { get; set; }

        [Required] 
        [StringLength(100)]  
        public string nombre { get; set; }

        [Required]  
        [EmailAddress]  
        public string email { get; set; }

        public string telefono { get; set; }           

        public string direccion { get; set; }          

        [Required]  
        public TipoUsuario tipoUsuario { get; set; }

        public string TipoUsuarioString => tipoUsuario.ToString();

        public string estado { get; set; }

        [Required]
        [StringLength(256)]
        public string contrasena { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; } = DateTime.Now;   

 
        public ICollection<Cuenta> cuentas { get; set; }
    }
}
