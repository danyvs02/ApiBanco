using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;
using ApiBanco.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ApiBanco.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaRepositorio _cuentaRepo;
        private readonly IMapper _mapper;

        public CuentasController(ICuentaRepositorio cuentaRepo, IMapper mapper)
        {
            _cuentaRepo = cuentaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCuentas()
        {
            var listaCuentas = _cuentaRepo.GetCuentas();

            var listaCuentasDto = new List<CuentaDto>();

            foreach (var lista in listaCuentas)
            {
                listaCuentasDto.Add(_mapper.Map<CuentaDto>(lista));
            }

            return Ok(listaCuentasDto);
        }

        [HttpGet("{cuentaId:int}", Name = "GetCuenta")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCuenta(int cuentaId)
        {
            var itemCuenta = _cuentaRepo.GetCuenta(cuentaId);

            if (itemCuenta == null)
            {
                return NotFound();
            }

            var itemCuentaDto = _mapper.Map<CuentaDto>(itemCuenta);
            return Ok(itemCuentaDto);
        }

        [HttpPost("crear")]
        [ProducesResponseType(201, Type = typeof(CuentaCrearDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCuenta([FromBody] CuentaCrearDto cuentaCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cuentaCrearDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_cuentaRepo.ExisteCuenta(cuentaCrearDto.id))
            {
                ModelState.AddModelError("", "La cuenta ya existe");
                return StatusCode(404, ModelState);
            }

            var cuenta = _mapper.Map<Cuenta>(cuentaCrearDto);

            if (!_cuentaRepo.CrearCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{cuenta.id}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCuenta", new { cuentaId = cuenta.id }, cuenta);
        }

    }
}

