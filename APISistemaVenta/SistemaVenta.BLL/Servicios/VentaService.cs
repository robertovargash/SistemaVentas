using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
  public class VentaService : IVentaService
  {
    private readonly IGenericRepository<DetalleVenta> detalleVentaRepositorio;
    private readonly IMapper mapper;
    private readonly IVentaRepository ventaRepository;

    public VentaService( IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper, IVentaRepository ventaRepository )
    {
      this.detalleVentaRepositorio = detalleVentaRepositorio;
      this.mapper = mapper;
      this.ventaRepository = ventaRepository;
    }

    public async Task<VentaDTO> Registrar( VentaDTO modelo )
    {
      try
      {
        var ventaGenerada = await ventaRepository.Registrar(mapper.Map<Venta>( modelo));
        if( ventaGenerada.IdVenta == 0 )
        {
          throw new TaskCanceledException( "No se pudo crear la venta" );
        }
        return mapper.Map<VentaDTO>( ventaGenerada );
      }
      catch( Exception )
      {
        throw;
      }
    }

    public async Task<List<VentaDTO>> Historial( string buscarPor, string numeroVenta, string fechaInicio, string fechaFin )
    {
      IQueryable<Venta> query = await ventaRepository.Consultar();
      var ListaResultado = new List<Venta>();
      try
      {
        if( buscarPor == "fecha" )
        {
          DateTime fecha_Inicio = DateTime.ParseExact( fechaInicio, "dd/MM/yyyy", new CultureInfo( "en-US" ));
          DateTime fecha_Fin = DateTime.ParseExact( fechaFin, "dd/MM/yyyy", new CultureInfo( "en-US" ) );
          ListaResultado = await query.Where( v =>
              v.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
              v.FechaRegistro.Value.Date >= fecha_Fin.Date
            ).Include(dv =>dv.DetalleVenta)
            .ThenInclude(p => p.IdProductoNavigation).ToListAsync();
        }
        else
        {
          ListaResultado = await query.Where( v =>
              v.NumeroDocumento == numeroVenta )
            .Include( dv => dv.DetalleVenta )
            .ThenInclude( p => p.IdProductoNavigation ).ToListAsync();
        }
        
      }
      catch( Exception )
      {
        throw;
      }
      return mapper.Map<List<VentaDTO>>( ListaResultado );
    }   

    public async Task<List<ReporteDTO>> Reporte( string fechaInicio, string fechaFin )
    {
      IQueryable<DetalleVenta> query = await detalleVentaRepositorio.Consultar();
      var ListaResultado = new List<DetalleVenta>();
      try
      {
        DateTime fecha_Inicio = DateTime.ParseExact( fechaInicio, "dd/MM/yyyy", new CultureInfo( "en-US" ) );
        DateTime fecha_Fin = DateTime.ParseExact( fechaFin, "dd/MM/yyyy", new CultureInfo( "en-US" ) );
        ListaResultado = await query
          .Include(p => p.IdProductoNavigation)
          .Include(v => v.IdVentaNavigation)
          .Where(dv => 
                dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_Inicio &&
                dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_Fin ).ToListAsync();
      }
      catch( Exception )
      {
        throw;
      }
      return mapper.Map<List<ReporteDTO>>( ListaResultado );
    }
  }
}
