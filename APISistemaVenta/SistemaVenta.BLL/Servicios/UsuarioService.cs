using SistemaVenta.DTO;
using AutoMapper;
using SistemaVenta.BLL;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using SistemaVenta.BLL.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.BLL.Servicios
{
  public class UsuarioService : IUsuarioService
  {
    private readonly IGenericRepository<Usuario> usuarioRepositorio;
    private readonly IMapper mapper;

    public UsuarioService( IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper )
    {
      this.usuarioRepositorio = usuarioRepositorio;
      this.mapper = mapper;
    }

    public async Task<List<UsuarioDTO>> Lista()
    {
      try
      {
        var quesyUsuarios = await usuarioRepositorio.Consultar();
        var listaUsuarios = quesyUsuarios.Include(rol => rol.IdRolNavigation).ToList();
        return mapper.Map<List<UsuarioDTO>>( listaUsuarios.ToList() );
      }
      catch( Exception )
      {

        throw;
      }
    }

    public async Task<SesionDTO> ValidarCredenciales( string correo, string clave )
    {
      try
      {
        var queryusuario = await usuarioRepositorio.Consultar( filtro => filtro.Correro == correo && filtro.Clave == clave );
        if( queryusuario.FirstOrDefault() == null )
        {
          throw new TaskCanceledException( "Usuario no existe" );
        }
        var devolverUsuario = queryusuario.Include(rol => rol.IdRolNavigation).First();
        return mapper.Map<SesionDTO>( devolverUsuario );
      }
      catch( Exception )
      {
        throw;
      }
    }

    public async Task<UsuarioDTO> Crear( UsuarioDTO modelo )
    {
      try
      {
        var usuariocreado = await usuarioRepositorio.Crear(mapper.Map<Usuario>( modelo));
        if( usuariocreado.Idusuario == 0 )
        {
          throw new TaskCanceledException( "No se pudo crear usuario" );
        }
        var query = await usuarioRepositorio.Consultar(u => u.Idusuario == usuariocreado.Idusuario );

        usuariocreado = query.Include( rol => rol.IdRolNavigation ).First();

        return mapper.Map<UsuarioDTO>( usuariocreado );
      }
      catch( Exception )
      {
        throw;
      }
    }

    public async Task<bool> Editar( UsuarioDTO modelo )
    {
      try
      {
        var usuarioModelo = mapper.Map<Usuario>( modelo );
        var usuarioEncontrado = await usuarioRepositorio.Obtener( u => u.Idusuario == usuarioModelo.Idusuario );
        if( usuarioEncontrado == null )
        {
          throw new TaskCanceledException( "No existe el usuario" );
        }
        usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
        usuarioEncontrado.Correro = usuarioModelo.Correro;
        usuarioEncontrado.IdRol = usuarioModelo.IdRol;
        usuarioEncontrado.Clave = usuarioModelo.Clave;
        usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
        bool respuesta = await usuarioRepositorio.Editar( usuarioEncontrado );
        if( !respuesta )
        {
          throw new TaskCanceledException( "No se pudo editar" );
        }
        return respuesta;
      }
      catch( Exception )
      {
        throw;
      }
    }

    public async Task<bool> Eliminar( int id )
    {
      try
      {
        var usuarioEncontrado = await usuarioRepositorio.Obtener( u => u.Idusuario == id );
        if( usuarioEncontrado == null )
        {
          throw new TaskCanceledException( "No existe el usuario" );
        }
        bool respuesta = await usuarioRepositorio.Eliminar( usuarioEncontrado );
        if( !respuesta )
        {
          throw new TaskCanceledException( "No se pudo eliminar" );
        }
        return respuesta;
      }
      catch( Exception )
      {
        throw;
      }      
    }    
  }
}
