using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class ProductoService : IProductoService
  {
    private readonly IGenericRepository<Producto> productoRepositorio;
    private readonly IMapper mapper;

    public ProductoService( IGenericRepository<Producto> productoRepositorio, IMapper mapper )
    {
      this.productoRepositorio = productoRepositorio;
      this.mapper = mapper;
    }

    public async Task<List<ProductoDTO>> Lista()
    {
      try
      {
        var queryproducto = await productoRepositorio.Consultar();
        var listaProductos = queryproducto.Include( cat => cat.IdCategoriaNavigation ).ToList();
        return mapper.Map<List<ProductoDTO>>( listaProductos.ToList() );
      }
      catch( Exception )
      {
        throw;
      }
    }
    public async Task<ProductoDTO> Crear( ProductoDTO modelo )
    {
      try
      {
        var productoCreado = await productoRepositorio.Crear( mapper.Map<Producto>( modelo ) );
        if( productoCreado.IdProducto == 0 )
        {
          throw new TaskCanceledException( "No se pudo crear el producto" );
        }
        //var query = await productoRepositorio.Consultar( u => u.IdProducto == productoCreado.IdProducto );
        //productoCreado = query.Include( cat => cat.IdCategoriaNavigation ).First();
        return mapper.Map<ProductoDTO>( productoCreado );
      }
      catch( Exception )
      {
        throw;
      }
      
    }

    public async Task<bool> Editar( ProductoDTO modelo )
    {
      try
      {
        var productomodelo = mapper.Map<Producto>( modelo );        
        var productoEncontrado = await productoRepositorio.Obtener( p => p.IdProducto == productomodelo.IdProducto ) ?? throw new TaskCanceledException( "No existe el producto" );
        
        productoEncontrado.Stock = productomodelo.Stock;
        productoEncontrado.Nombre = productomodelo.Nombre;
        productoEncontrado.Precio = productomodelo.Precio;
        productoEncontrado.EsActivo = productomodelo.EsActivo;
        productoEncontrado.IdCategoria = productomodelo.IdCategoria;

        bool respuesta = await productoRepositorio.Editar( productoEncontrado );
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
        var productoEncontrado = await productoRepositorio.Obtener( p => p.IdProducto == id );
        if( productoEncontrado == null )
        {
          throw new TaskCanceledException( "No existe el producto" );
        }
        bool respuesta = await productoRepositorio.Eliminar( productoEncontrado );
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
