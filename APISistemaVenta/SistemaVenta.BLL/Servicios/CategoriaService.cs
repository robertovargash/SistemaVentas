using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using AutoMapper;
using SistemaVenta.BLL;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class CategoriaService : ICategoriaService
  {
    private readonly IGenericRepository<Categoria> categoriaRepositorio;
    private readonly IMapper mapper;

    public CategoriaService( IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper )
    {
      this.categoriaRepositorio = categoriaRepositorio;
      this.mapper = mapper;
    }

    public async Task<List<CategoriaDTO>> Lista()
    {
      try
      {
        var listacategorias = await categoriaRepositorio.Consultar();
        return mapper.Map<List<CategoriaDTO>>( listacategorias.ToList() );
      }
      catch( Exception )
      {
        throw;
      }
    }
  }
}
