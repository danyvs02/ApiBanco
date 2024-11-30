using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;

namespace ApiBanco.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {

        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int usuarioId);
        bool IsUniqueUser(string usuarioId);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Crear(UsuarioCrearDto crearUsuarioDto);

        bool Guardar();
    }
}
