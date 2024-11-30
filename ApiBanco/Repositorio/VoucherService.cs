using ApiBanco.Modelos;

namespace ApiBanco.Repositorio
{
    public class VoucherService
    {
        public string GenerarVoucher(Transaccion transaccion)
        {
            var voucher = $@"
            ---------------------------
            Voucher de Transacción
            ---------------------------
            Fecha: {transaccion.fechaTransaccion}
            Monto: {transaccion.monto:C}
            Cuenta Origen: {transaccion.cuentaOrigen.numeroCuenta}
            Cuenta Destino: {transaccion.cuentaDestino.numeroCuenta}
            Tipo de Transacción: {transaccion.tipoTransaccion}
            Descripción: {transaccion.descripcion}
            Estado: {transaccion.estado}
            ---------------------------
        ";

            return voucher;
        }
    }
}
