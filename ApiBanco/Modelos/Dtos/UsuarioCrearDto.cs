using System.ComponentModel.DataAnnotations;

namespace ApiBanco.Modelos.Dtos
{
    public enum TipoUsuario
    {
        Cliente = 1,
        Empresa = 2,
        Administrador = 3
    }
    public class UsuarioCrearDto
    {

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string email { get; set; }

        public string telefono { get; set; }

        public string direccion { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es obligatorio")]
        [EnumDataType(typeof(TipoUsuario), ErrorMessage = "El tipo de usuario debe ser válido")]
        public TipoUsuario tipoUsuario { get; set; }

        public string estado { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(256)]
        public string contrasena { get; set; }

    }
}
