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
  public class UsuarioController : ControllerBase
  {
    private readonly IUsuarioService usuarioService;

    public UsuarioController( IUsuarioService usuarioService )
    {
      this.usuarioService = usuarioService;
    }

    [HttpGet( "Lista" )]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<UsuarioDTO>>();
      try
      {
        response.Status = true;
        response.Value = await usuarioService.Lista();
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpPost( "IniciarSesion" )]
    public async Task<IActionResult> IniciarSesion( [FromBody] LoginDTO login )
    {
      var response = new Response<SesionDTO>();
      try
      {
        response.Status = true;
        response.Value = await usuarioService.ValidarCredenciales( login.Correro, login.Clave );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpPost( "Guardar" )]
    public async Task<IActionResult> Guardar( [FromBody] UsuarioDTO usuario )
    {
      var response = new Response<UsuarioDTO>();
      try
      {
        response.Status = true;
        response.Value = await usuarioService.Crear( usuario );
      }
      catch( Exception ex )
      {
        response.Status = false;
        response.Msg = ex.Message;
      }
      return Ok( response );
    }

    [HttpPut( "Editar" )]
    public async Task<IActionResult> Editar( [FromBody] UsuarioDTO usuario )
    {
      var response = new Response<bool>();
      try
      {
        response.Status = true;
        response.Value = await usuarioService.Editar( usuario );
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
        response.Value = await usuarioService.Eliminar( id );
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
