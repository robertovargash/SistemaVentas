using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
  public class DashBoardService : IDashBoardService
  {
    private readonly IGenericRepository<Producto> productoRepositorio;
    private readonly IMapper mapper;
    private readonly IVentaRepository ventaRepository;

    public DashBoardService( IGenericRepository<Producto> productoRepositorio, IMapper mapper, IVentaRepository ventaRepository )
    {
      this.productoRepositorio = productoRepositorio;
      this.mapper = mapper;
      this.ventaRepository = ventaRepository;
    }

    public async Task<DashboardDTo> Resumen()
    {
      DashboardDTo vmDashBoard = new DashboardDTo();
      try
      {
        vmDashBoard.TotalVentas = await TotalventasUltimaSemana();
        vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
        vmDashBoard.TotalProductos = await TotalProductos();

        List<VentaSemanaDTO> listaVentaSemana = new List<VentaSemanaDTO>();
        foreach( KeyValuePair<string,int> item in await VentasUltimaSemana() )
        {
          listaVentaSemana.Add( new VentaSemanaDTO()
          {
            Fecha = item.Key,
            Total = item.Value
          } );
        }

        vmDashBoard.VentasUltimaSemana = listaVentaSemana;
      }
      catch( Exception )
      {
        throw;
      }

      return vmDashBoard;
    }

    private IQueryable<Venta> RetornarVentas( IQueryable<Venta> tablaVenta, int restarCantidadDias )
    {
      DateTime? ultimafecha = tablaVenta.OrderByDescending( v => v.FechaRegistro ).Select( v => v.FechaRegistro ).First();
      ultimafecha = ultimafecha.Value.AddDays( restarCantidadDias );
      return tablaVenta.Where( v => v.FechaRegistro.Value.Date >= ultimafecha.Value.Date );
    }

    private async Task<int> TotalventasUltimaSemana()
    {
      int total = 0;
      IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
      if( ventaQuery.Any() )
      {
        var tablaVenta = RetornarVentas( ventaQuery, -7 );
        total = tablaVenta.Count();
      }
      return total;
    }

    private async Task<string> TotalIngresosUltimaSemana()
    {
      decimal resultado = 0;
      IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
      if( ventaQuery.Any() )
      {
        var tablaVenta = RetornarVentas( ventaQuery, -7 );
        resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
      }
      return Convert.ToString(resultado, new CultureInfo("en-US"));
    }

    private async Task<int> TotalProductos()
    {
      IQueryable<Producto> productoQuery = await productoRepositorio.Consultar();
      int total = productoQuery.Count();
      return total;
    }

    private async Task<Dictionary<string,int>> VentasUltimaSemana()
    {
      Dictionary<string,int> resultado = new Dictionary<string,int>();
      IQueryable<Venta> ventaQuery = await ventaRepository.Consultar();
      if( ventaQuery.Any() )
      {
        var tablaVenta = RetornarVentas( ventaQuery, -7 );

        resultado = tablaVenta
          .GroupBy( v => v.FechaRegistro.Value.Date ).OrderBy( g => g.Key )
          .Select( dv => new { fecha = dv.Key.ToString( "dd/MM/yyyy" ), total = dv.Count() } )
          .ToDictionary( keySelector: alias => alias.fecha, elementSelector: alias => alias.total );

      }

      return resultado;

    }
  }
}
