using ApiBanco.Modelos;
using System.Collections;

namespace ApiBanco.Repositorio.IRepositorio
{
    public interface ICuentaRepositorio
    {
        ICollection<Cuenta> GetCuentas();
        Cuenta GetCuenta(int cuentaId);
        bool ExisteCuenta(int id);
        bool CrearCuenta(Cuenta cuenta);

        bool ActualizarCuenta(Cuenta cuenta);

        bool BorrarCuenta(Cuenta cuenta);

        bool Guardar();
    }
}
