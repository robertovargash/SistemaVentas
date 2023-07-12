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
  public class ProductoController : ControllerBase
  {
    private readonly IProductoService productoService;

    public ProductoController( IProductoService productoService )
    {
      this.productoService = productoService;
    }

    [HttpGet( "Lista" )]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<ProductoDTO>>();
      try
      {
        response.Status = true;
        response.Value = await productoService.Lista();
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpPost( "Crear" )]
    public async Task<IActionResult> Guardar( [FromBody] ProductoDTO producto )
    {
      var response = new Response<ProductoDTO>();
      try
      {
        response.Status = true;
        response.Value = await productoService.Crear( producto );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpPut( "Editar" )]
    public async Task<IActionResult> Editar( [FromBody] ProductoDTO producto )
    {
      var response = new Response<bool>();
      try
      {
        response.Status = true;
        response.Value = await productoService.Editar( producto );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpDelete( "Eliminar/{id:int}" )]
    public async Task<IActionResult> Eliminar( int id )
    {
      var response = new Response<bool>();
      try
      {
        response.Status = true;
        response.Value = await productoService.Eliminar( id );
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
