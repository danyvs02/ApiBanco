using ApiBanco.Data;
using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;
using ApiBanco.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiBanco.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;

        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.usuarios.FirstOrDefault(c => c.id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.usuarios.OrderBy(c => c.id).ToList();
        }

        public bool IsUniqueUser(string usuarioId)
        {
            var usuariobd = _bd.usuarios.FirstOrDefault(u => u.email == usuarioId);
            if (usuariobd == null)
            {
                return true;
            }

            return false;
        }
        

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.contrasena);

            var usuario = _bd.usuarios.FirstOrDefault(
                u => u.email.ToLower() == usuarioLoginDto.email.ToLower()
                && u.contrasena == passwordEncriptado
                );

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Aquí existe el usuario entonces podemos procesar el login      
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.email),
                    //new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };

            return usuarioLoginRespuestaDto;
        }




        public async Task<Usuario> Crear(UsuarioCrearDto usuarioCrearDto)
        {
            var passwordEncriptado = obtenermd5(usuarioCrearDto.contrasena);

            Usuario usuario = new Usuario()
            {
                email = usuarioCrearDto.email,
                nombre = usuarioCrearDto.nombre,
                contrasena = usuarioCrearDto.contrasena,
                telefono = usuarioCrearDto.telefono,
                direccion = usuarioCrearDto.direccion, // Asignar la dirección
                tipoUsuario = usuarioCrearDto.tipoUsuario,
                estado = "Activo"
            };

            ////Validar si el usuario ya existe
            //var nombreUsuarioDesdeBd = _bd.Usuario.FirstOrDefault(
            //    u => u.NombreUsuario == usuarioRegistroDto.NombreUsuario);

            //if (nombreUsuarioDesdeBd == null)
            //{
            //    return usuario;
            //}
            
            _bd.usuarios.Add(usuario);
            usuario.contrasena = passwordEncriptado;
            await _bd.SaveChangesAsync();
            return usuario;
        }


        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
