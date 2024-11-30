using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class ActualizarUsuarioDto
    {
 
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string email { get; set; }

        public string telefono { get; set; }

        public string direccion { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es obligatorio")]
        public string tipoUsuario { get; set; }

        public string estado { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(256)]
        public string contrasena { get; set; }

    }
}
