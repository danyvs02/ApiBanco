using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public class UsuarioLoginDto
    {

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(256)]
        public string contrasena { get; set; }
    }
}
