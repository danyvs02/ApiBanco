using ApiBanco.Modelos.Dtos;
using ApiBanco.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBanco.Controllers
{
    [Route("api/transacciones")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {

        private readonly ITransaccionRepositorio _transaccionRepo;
        private readonly IMapper _mapper;

        public TransaccionesController(ITransaccionRepositorio transaccionRepo, IMapper mapper)
        {
            _transaccionRepo = transaccionRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTransacciones()
        {
            var listaTransacciones = _transaccionRepo.GetTransacciones();

            var listaTransaccionesDto = new List<TransaccionDto>();

            foreach (var lista in listaTransacciones)
            {
                listaTransaccionesDto.Add(_mapper.Map<TransaccionDto>(lista));
            }

            return Ok(listaTransaccionesDto);
        }

        [HttpGet("{transaccionId:int}", Name = "GetTransaccion")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTransaccion(int transaccionId)
        {
            var itemTransaccion = _transaccionRepo.GetTransaccion(transaccionId);

            if (itemTransaccion == null)
            {
                return NotFound();
            }

            var itemTransaccionDto = _mapper.Map<TransaccionDto>(itemTransaccion);
            return Ok(itemTransaccionDto);
        }

        // Endpoint para realizar un retiro
        [HttpPost("retiro")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Retirar([FromBody] TransaccionRetirarDto transaccionRetirarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = _transaccionRepo.RetirarTransaccion(transaccionRetirarDto);

            if (!resultado)
            {
                return BadRequest("No se pudo procesar el retiro. Verifique el saldo de la cuenta.");
            }

            return Ok("Retiro procesado con éxito.");
        }

        // Endpoint para realizar un depósito
        [HttpPost("deposito")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Depositar([FromBody] TransaccionDepositarDto transaccionDepositarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = _transaccionRepo.DepositarTransaccion(transaccionDepositarDto);

            if (!resultado)
            {
                return BadRequest("No se pudo procesar el depósito.");
            }

            return Ok("Depósito procesado con éxito.");
        }

        // Endpoint para realizar una transferencia
        [HttpPost("transferir")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Transferir([FromBody] TransaccionTransferirDto transaccionTransferirDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = _transaccionRepo.TransferirTransaccion(transaccionTransferirDto);

            if (!resultado)
            {
                return BadRequest("No se pudo procesar la transferencia. Verifique las cuentas y el saldo.");
            }

            return Ok("Transferencia procesada con éxito.");
        }
    }
}
