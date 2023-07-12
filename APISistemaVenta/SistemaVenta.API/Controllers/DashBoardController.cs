using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class DashBoardController : ControllerBase
  {
    private readonly IDashBoardService dashboardService;

    public DashBoardController( IDashBoardService dashboardService )
    {
      this.dashboardService = dashboardService;
    }

    [HttpGet( "Dashboard" )]
    public async Task<IActionResult> Dashboard()
    {
      var response = new Response<DashboardDTo>();
      try
      {
        response.Status = true;
        response.Value = await dashboardService.Resumen(); ;
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
