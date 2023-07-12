using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class VentaController : ControllerBase
  {
    private readonly IVentaService ventaService;

    public VentaController( IVentaService ventaService )
    {
      this.ventaService = ventaService;
    }

    [HttpPost( "Registrar" )]
    public async Task<IActionResult> Registrar( [FromBody] VentaDTO venta )
    {
      var response = new Response<VentaDTO>();
      try
      {
        response.Status = true;
        response.Value = await ventaService.Registrar( venta );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpGet( "Historial" )]
    public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
    {
      var response = new Response<List<VentaDTO>>();
      numeroVenta = numeroVenta is null ? "" : numeroVenta;
      fechaInicio = fechaInicio is null ? "" : fechaInicio;
      fechaFin = fechaFin is null ? "" : fechaFin;
      try
      {
        response.Status = true;
        response.Value = await ventaService.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpGet( "Reporte" )]
    public async Task<IActionResult> Reporte( string? fechaInicio, string? fechaFin )
    {
      var response = new Response<List<ReporteDTO>>();
      fechaInicio = fechaInicio is null ? "" : fechaInicio;
      fechaFin = fechaFin is null ? "" : fechaFin;
      try
      {
        response.Status = true;
        response.Value = await ventaService.Reporte( fechaInicio, fechaFin );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }
  }
}
