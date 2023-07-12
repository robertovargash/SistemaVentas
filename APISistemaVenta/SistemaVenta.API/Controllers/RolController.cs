﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class RolController : ControllerBase
  {
    private readonly IRolService _rolService;

    public RolController( IRolService rolService )
    {
      _rolService = rolService;
    }

    [HttpGet("Lista")]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<RolDTO>>();
      try
      {
        response.Status = true;
        response.Value = await _rolService.Lista();
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
