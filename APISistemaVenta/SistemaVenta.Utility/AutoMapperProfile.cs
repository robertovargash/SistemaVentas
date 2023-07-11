using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.Utility
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<Rol, RolDTO>().ReverseMap();

      CreateMap<Menu, MenuDTO>().ReverseMap();

      CreateMap<Usuario, UsuarioDTO>()
        .ForMember( destino =>
        destino.RolDescripcion,
        option => option.MapFrom( origen => origen.IdRolNavigation.Nombre ) )
        .ForMember( destino => destino.EsActivo,
        option => option.MapFrom( origen => origen.EsActivo == true ? 1 : 0 ) );

      CreateMap<Usuario, SesionDTO>()
             .ForMember( destino =>
             destino.RolDescripcion,
             option => option.MapFrom( origen => origen.IdRolNavigation.Nombre ) );

      CreateMap<UsuarioDTO, Usuario>()
             .ForMember( destino =>
              destino.IdRolNavigation,
              option => option.Ignore() )
              .ForMember( destino => destino.EsActivo,
              option => option.MapFrom( origen => origen.EsActivo == 1 ? true : false ) );

      CreateMap<Categoria, CategoriaDTO>().ReverseMap();

      CreateMap<Producto, ProductoDTO>().ForMember( destino =>
        destino.DescripcionCategoria,
        option => option.MapFrom( origen => origen.IdCategoriaNavigation.Nombre ) )
        .ForMember( destino => destino.EsActivo,
        option => option.MapFrom( origen => origen.EsActivo == true ? 1 : 0 ) )
        .ForMember( destino => destino.Precio,
        option => option.MapFrom( origen => Convert.ToString( origen.Precio.Value, new CultureInfo( "en-US" ) ) ) );

      CreateMap<ProductoDTO, Producto>()
        .ForMember( destino =>
       destino.IdCategoriaNavigation,
       option => option.Ignore() )
       .ForMember( destino => destino.EsActivo,
       option => option.MapFrom( origen => origen.EsActivo == 1 ? true : false ) )
       .ForMember( destino => destino.Precio,
       option => option.MapFrom( origen => Convert.ToDecimal( origen.Precio, new CultureInfo( "en-US" ) ) ) );

      CreateMap<Venta, VentaDTO>()
        .ForMember( destino => destino.TotalTexto,
        option => option.MapFrom( origen => Convert.ToString( origen.Total.Value, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.FechaRegistro,
        option => option.MapFrom( origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy") ) );

      CreateMap<VentaDTO,Venta>()
        .ForMember( destino => destino.Total,
        option => option.MapFrom( origen => Convert.ToDecimal( origen.TotalTexto, new CultureInfo( "en-US" ) ) ) );

      CreateMap<DetalleVenta, DetalleVentaDTO>()
        .ForMember( destino => destino.PrecioTexto,
        option => option.MapFrom( origen => Convert.ToString( origen.Precio, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.TotalTexto,
        option => option.MapFrom( origen => Convert.ToString( origen.Total, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.DescripcionProducto,
        option => option.MapFrom( origen => origen.IdProductoNavigation.Nombre ) );

      CreateMap<DetalleVentaDTO, DetalleVenta>()
        .ForMember( destino => destino.Precio,
        option => option.MapFrom( origen => Convert.ToDecimal( origen.PrecioTexto, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.Total,
        option => option.MapFrom( origen => Convert.ToDecimal( origen.TotalTexto, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.IdProductoNavigation,
        option => option.Ignore() );

      CreateMap<DetalleVenta, ReporteDTO>().ForMember( destino => destino.FechaRegistro,
        option => option.MapFrom( origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString( "dd/MM/yyyy" ) ) )
        .ForMember( destino => destino.NumeroDocumento,
        option => option.MapFrom( origen => origen.IdVentaNavigation.NumeroDocumento ) )
        .ForMember( destino => destino.TipoPago,
        option => option.MapFrom( origen => origen.IdVentaNavigation.TipoPago ) )
        .ForMember( destino => destino.TotalVenta,
        option => option.MapFrom( origen => Convert.ToString( origen.IdVentaNavigation.Total.Value, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.Producto,
        option => option.MapFrom( origen => origen.IdProductoNavigation.Nombre ) )
        .ForMember( destino => destino.Precio,
        option => option.MapFrom( origen => Convert.ToString( origen.IdProductoNavigation.Precio.Value, new CultureInfo( "en-US" ) ) ) )
        .ForMember( destino => destino.Total,
        option => option.MapFrom( origen => Convert.ToString( origen.Total.Value, new CultureInfo( "en-US" ) ) ) );


    }
  }
}
