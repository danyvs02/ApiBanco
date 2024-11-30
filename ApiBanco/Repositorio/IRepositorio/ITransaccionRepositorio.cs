using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;

namespace ApiBanco.Repositorio.IRepositorio
{
    public interface ITransaccionRepositorio
    {
        ICollection<Transaccion> GetTransacciones();
        Transaccion GetTransaccion(int transaccionId);
        bool TransferirTransaccion(TransaccionTransferirDto transferirTransaccionDto);

        bool DepositarTransaccion(TransaccionDepositarDto depositarTransaccionDto);

        bool RetirarTransaccion(TransaccionRetirarDto retirartransaccionDto);

        bool Guardar();

    }
}
