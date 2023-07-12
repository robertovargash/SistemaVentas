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
  public class CategoriaController : ControllerBase
  {
    private readonly ICategoriaService categoriaService;

    public CategoriaController( ICategoriaService categoriaService )
    {
      this.categoriaService = categoriaService;
    }

    [HttpGet( "Lista" )]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<CategoriaDTO>>();
      try
      {
        response.Status = true;
        response.Value = await categoriaService.Lista();
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
