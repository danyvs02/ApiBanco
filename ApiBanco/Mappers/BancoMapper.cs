using ApiBanco.Modelos;
using ApiBanco.Modelos.Dtos;
using AutoMapper;

namespace ApiBanco.Mappers
{
    public class BancoMapper : Profile
    {
        public BancoMapper() {
        
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioCrearDto>().ReverseMap();

            CreateMap<CuentaCrearDto, Cuenta>()
            .ForMember(dest => dest.clave, opt => opt.Ignore());  // Ignorar el mapeo de la clave

            CreateMap<Cuenta, CuentaDto>().ReverseMap();
            CreateMap<Cuenta, CuentaCrearDto>().ReverseMap();



            CreateMap<Transaccion, TransaccionDto>()
    .ForMember(dest => dest.numeroCuentaOrigen,
        opt => opt.MapFrom(src => src.cuentaOrigen != null ? src.cuentaOrigen.numeroCuenta : null))  // Verifica si cuentaOrigen es null
    .ForMember(dest => dest.numeroCuentaDestino,
        opt => opt.MapFrom(src => src.cuentaDestino != null ? src.cuentaDestino.numeroCuenta : null));  // Verifica si cuentaDestino es null

            CreateMap<Transaccion, TransaccionTransferirDto>().ReverseMap();
            CreateMap<Transaccion, TransaccionDepositarDto>().ReverseMap();
            CreateMap<Transaccion, TransaccionRetirarDto>().ReverseMap();

        }    
    }
}
