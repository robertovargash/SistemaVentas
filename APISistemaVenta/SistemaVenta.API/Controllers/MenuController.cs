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
  public class MenuController : ControllerBase
  {
    private readonly IMenuService menuService;

    public MenuController( IMenuService menuService )
    {
      this.menuService = menuService;
    }

    [HttpGet( "Lista" )]
    public async Task<IActionResult> Lista(int idUsuario)
    {
      var response = new Response<List<MenuDTO>>();
      try
      {
        response.Status = true;
        response.Value = await menuService.Lista(idUsuario); ;
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
