using ApiBanco.Data;
using ApiBanco.Modelos;
using ApiBanco.Repositorio.IRepositorio;
using Microsoft.Extensions.Hosting;

namespace ApiBanco.Repositorio
{
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public CuentaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarCuenta(Cuenta cuenta)
        {
            _bd.cuentas.Update(cuenta);
            return Guardar();
        }

        public bool BorrarCuenta(Cuenta cuenta)
        {
            _bd.cuentas.Remove(cuenta);
            return Guardar();
        }

        public bool CrearCuenta(Cuenta cuenta)
        {
            cuenta.fechaCreacion = DateTime.Now;
            // Asignar valores predeterminados si no se proporcionan
            if (string.IsNullOrEmpty(cuenta.tipoCuenta))
            {
                cuenta.tipoCuenta = "Debito"; // Asignar valor predeterminado
            }

            if (string.IsNullOrEmpty(cuenta.estado))
            {
                cuenta.estado = "Activo"; // Asignar valor predeterminado
            }

            // Generamos el número de cuenta único
            cuenta.numeroCuenta = GenerarNumeroCuenta();

            // Verificamos que el número de cuenta sea único
            while (_bd.cuentas.Any(c => c.numeroCuenta == cuenta.numeroCuenta))
            {
                cuenta.numeroCuenta = GenerarNumeroCuenta(); // Generamos un nuevo número si ya existe uno igual
            }

            // Encriptar la clave (PIN)
            cuenta.clave = ObtenerMD5(cuenta.clave); // Aquí encriptamos el PIN

            _bd.cuentas.Add(cuenta);
            return Guardar();
        }

        private string GenerarNumeroCuenta()
        {
            Random random = new Random();
            // Generamos un número aleatorio de 16 dígitos
            long numeroCuenta = (long)(random.NextDouble() * (9999999999999999L - 1000000000000000L) + 1000000000000000L);
            return numeroCuenta.ToString();
        }

        public bool ExisteCuenta(int id)
        {
            return _bd.cuentas.Any(c => c.id == id);
        }

        public Cuenta GetCuenta(int cuentaId)
        {
            return _bd.cuentas.FirstOrDefault(c => c.id == cuentaId);
        }

        public ICollection<Cuenta> GetCuentas()
        {
            return _bd.cuentas.OrderBy(c => c.id).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }

        public static string ObtenerMD5(string valor)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(valor));
                var sb = new System.Text.StringBuilder();
                foreach (var byteValue in data)
                {
                    sb.Append(byteValue.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
