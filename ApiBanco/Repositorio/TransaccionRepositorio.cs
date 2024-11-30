using ApiBanco.Data;
using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;
using ApiBanco.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace ApiBanco.Repositorio
{
    public class TransaccionRepositorio : ITransaccionRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly Notificador _notificador;
        private readonly VoucherService _voucherService;

        public TransaccionRepositorio(ApplicationDbContext bd, Notificador notificador, VoucherService voucherService)
        {
            _bd = bd;
            _notificador = notificador;
            _voucherService = voucherService;

        }
        public bool DepositarTransaccion(TransaccionDepositarDto transaccionDepositarDto)
        {
            var cuentaDestino = _bd.cuentas.FirstOrDefault(c => c.numeroCuenta == transaccionDepositarDto.numeroCuentaDestino);
            if (cuentaDestino == null)
            {
                return false;
            }

            // Crear la transacción
            var transaccion = new Transaccion
            {
                cuentaOrigenId = cuentaDestino.id, // Para el depósito, la cuentaOrigen es la misma que la cuentaDestino.
                cuentaDestinoId = cuentaDestino.id,
                tipoTransaccion = "Deposito",
                monto = transaccionDepositarDto.monto,
                fechaTransaccion = DateTime.Now,
                estado = "Completada",
                descripcion = transaccionDepositarDto.descripcion
            };

            // Actualizar el saldo de la cuenta de destino
            cuentaDestino.saldo += transaccionDepositarDto.monto;

            // Guardar la transacción y los cambios
            _bd.transacciones.Add(transaccion);
            if (Guardar())
            {
                // Generar el voucher para la transacción completada
                var voucher = _voucherService.GenerarVoucher(transaccion);

                // Enviar notificación de éxito
                _notificador.EnviarNotificacion($"Depósito de {transaccionDepositarDto.monto:C} completado exitosamente. Voucher: \n{voucher}");

                return true;
            }
            _notificador.EnviarNotificacion("Error al procesar el depósito.");
            return false;

        }

        public Transaccion GetTransaccion(int transaccionId)
        {
            return _bd.transacciones
                       .Include(t => t.cuentaOrigen)  // Asegúrate de incluir la cuentaOrigen
                       .Include(t => t.cuentaDestino) // Asegúrate de incluir la cuentaDestino
                       .FirstOrDefault(t => t.id == transaccionId);
        }

        public ICollection<Transaccion> GetTransacciones()
        {
            return _bd.transacciones.Include(t => t.cuentaOrigen)
                                    .Include(t => t.cuentaDestino)
                                    .OrderByDescending(t => t.fechaTransaccion)
                                    .ToList();
        }




        public bool RetirarTransaccion(TransaccionRetirarDto transaccionRetirarDto)
        {
            var cuentaOrigen = _bd.cuentas.FirstOrDefault(c => c.numeroCuenta == transaccionRetirarDto.numeroCuentaOrigen);
            if (cuentaOrigen == null || cuentaOrigen.saldo < transaccionRetirarDto.monto)
            {
                // Si la cuenta no existe o no tiene suficiente saldo, no se puede realizar el retiro
                return false;
            }

            // Debugging: Verificar el valor de la clave
            Console.WriteLine($"Clave proporcionada: {transaccionRetirarDto.clave}");
            Console.WriteLine($"Clave almacenada en la cuenta: {cuentaOrigen.clave}");
            // Validar la clave

            // Verificar la clave usando MD5 (sin usar BCrypt)
            if (cuentaOrigen.clave != ObtenerMD5(transaccionRetirarDto.clave))
            {
                return false; // Clave incorrecta
            }

            // Crear la transacción de retiro
            var transaccion = new Transaccion
            {
                cuentaOrigenId = cuentaOrigen.id,
                cuentaDestinoId = cuentaOrigen.id, // La cuentaDestino es la misma en un retiro
                tipoTransaccion = "Retiro",
                monto = transaccionRetirarDto.monto,
                fechaTransaccion = DateTime.Now,
                estado = "Completada",
                descripcion = transaccionRetirarDto.descripcion,
                clave = ObtenerMD5(transaccionRetirarDto.clave)  // Guardar la clave con la transacción
            };

            // Actualizar el saldo de la cuenta de origen
            cuentaOrigen.saldo -= transaccionRetirarDto.monto;

            // Guardar la transacción y los cambios
            _bd.transacciones.Add(transaccion);
            if (Guardar())
            {
                // Generar el voucher para la transacción completada
                var voucher = _voucherService.GenerarVoucher(transaccion);

                // Enviar notificación de éxito
                _notificador.EnviarNotificacion($"Retiro de {transaccionRetirarDto.monto:C} completado exitosamente. Voucher: \n{voucher}");

                return true;
            }

            // Enviar notificación en caso de error
            _notificador.EnviarNotificacion("Error al procesar el retiro.");
            return false;
        }

        public bool TransferirTransaccion(TransaccionTransferirDto transaccionTransferirDto)
        {
            var cuentaOrigen = _bd.cuentas.FirstOrDefault(c => c.numeroCuenta == transaccionTransferirDto.numeroCuentaOrigen);
            var cuentaDestino = _bd.cuentas.FirstOrDefault(c => c.numeroCuenta == transaccionTransferirDto.numeroCuentaDestino);

            if (cuentaOrigen == null)
            {
                _notificador.EnviarNotificacion("La cuenta de origen no existe.");
                return false;
            }

            if (cuentaDestino == null)
            {
                _notificador.EnviarNotificacion("La cuenta de destino no existe.");
                return false;
            }

            if (cuentaOrigen.saldo < transaccionTransferirDto.monto)
            {
                _notificador.EnviarNotificacion("Saldo insuficiente en la cuenta de origen.");
                return false;
            }

            // Verificar la clave usando MD5 (sin usar BCrypt)
            if (cuentaOrigen.clave != ObtenerMD5(transaccionTransferirDto.clave))
            {
                return false; // Clave incorrecta
            }


            // Crear la transacción de transferencia
            var transaccion = new Transaccion
            {
                cuentaOrigenId = cuentaOrigen.id,
                cuentaDestinoId = cuentaDestino.id,
                tipoTransaccion = "Transferencia",
                monto = transaccionTransferirDto.monto,
                fechaTransaccion = DateTime.Now,
                estado = "Completada",
                descripcion = transaccionTransferirDto.descripcion,
                clave = ObtenerMD5(transaccionTransferirDto.clave) // Guardar la clave con la transacción
            };

            // Actualizar los saldos de ambas cuentas
            cuentaOrigen.saldo -= transaccionTransferirDto.monto;
            cuentaDestino.saldo += transaccionTransferirDto.monto;

            // Guardar la transacción y los cambios
            _bd.transacciones.Add(transaccion);

            if (Guardar())
            {
                // Generar el voucher para la transacción completada
                var voucher = _voucherService.GenerarVoucher(transaccion);

                // Enviar notificación de éxito
                _notificador.EnviarNotificacion($"Transferencia de {transaccionTransferirDto.monto:C} completada exitosamente. Voucher: \n{voucher}");

                return true;
            }

            // Enviar notificación en caso de error
            _notificador.EnviarNotificacion("Error al procesar la transferencia.");
            return false;
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

        public bool Guardar()
        {
            try
            {
                return _bd.SaveChanges() >= 0;
            }
            catch (Exception ex)
            {
                // Loguear el error
                Console.WriteLine($"Error al guardar cambios: {ex.Message}");
                return false;
            }
        }
    }
}