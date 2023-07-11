using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
  public class MenuService : IMenuService
  {
    private readonly IGenericRepository<Usuario> usuarioRepositorio;
    private readonly IGenericRepository<MenuRol> menuRolRepositorio;
    private readonly IGenericRepository<Menu> menuRepositorio;
    private readonly IMapper mapper;

    public MenuService( IGenericRepository<Usuario> usuarioRepositorio, IGenericRepository<MenuRol> menuRolRepositorio, IGenericRepository<Menu> menuRepositorio, IMapper mapper )
    {
      this.usuarioRepositorio = usuarioRepositorio;
      this.menuRolRepositorio = menuRolRepositorio;
      this.menuRepositorio = menuRepositorio;
      this.mapper = mapper;
    }

    public async Task<List<MenuDTO>> Lista( int idUsuario )
    {
      IQueryable<Usuario> tbUsuario = await usuarioRepositorio.Consultar(u=>u.Idusuario == idUsuario);
      IQueryable<MenuRol> tbMenuRol = await menuRolRepositorio.Consultar();
      IQueryable<Menu> tbMenu = await menuRepositorio.Consultar();

      try
      {
        IQueryable<Menu> tbResultado = (from u in tbUsuario
                                        join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                        join m in tbMenu on mr.IdMenu equals m.IdMenu
                                        select m).AsQueryable();

        var listaMenus = tbResultado.ToList();
        return mapper.Map<List<MenuDTO>>(listaMenus);
      }
      catch( Exception )
      {

        throw;
      }

    }
  }
}
