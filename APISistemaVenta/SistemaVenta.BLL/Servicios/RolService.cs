using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using AutoMapper;
using SistemaVenta.BLL;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;


namespace SistemaVenta.BLL.Servicios
{
  public class RolService : IRolService
  {
    private readonly IGenericRepository<Rol> rolRepositorio;
    private readonly IMapper mapper;

    public RolService( IGenericRepository<Rol> rolRepositorio, IMapper mapper )
    {
      this.rolRepositorio = rolRepositorio;
      this.mapper = mapper;
    }

    public async Task<List<RolDTO>> Lista()
    {
      try
      {
        var listaRoles = await rolRepositorio.Consultar();
        return mapper.Map<List<RolDTO>>( listaRoles.ToList() );
      }
      catch( Exception )
      {

        throw;
      }    
    }
  }
}
