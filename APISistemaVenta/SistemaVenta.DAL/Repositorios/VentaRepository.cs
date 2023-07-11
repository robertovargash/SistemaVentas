using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.Model;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.DAL.Repositorios
{
  public class VentaRepository : GenericRepository<Venta>, IVentaRepository
  {
    private readonly DbventaContext _dbContext;

    public VentaRepository( DbventaContext dbContext ): base( dbContext )
    {
      _dbContext = dbContext;
    }

    public async Task<Venta> Registrar( Venta venta )
    {
      Venta ventaGenerada = new();
      using ( var transaction = _dbContext.Database.BeginTransaction() )
      {
        try
        {
          foreach( DetalleVenta dv in venta.DetalleVenta)
          {
            Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto ).First();
            producto_encontrado.Stock -= dv.Cantidad;
            _dbContext.Productos.Update( producto_encontrado );
          }
          await _dbContext.SaveChangesAsync();
          NumeroDocumento correlativo = _dbContext.NumeroDocumentos.First();
          correlativo.UltimoNumero++;
          correlativo.FechaRegistro = DateTime.Now;
          _dbContext.NumeroDocumentos.Update( correlativo );
          await _dbContext.SaveChangesAsync();

          int cantidadDigitos = 4;
          string ceros = string.Concat( Enumerable.Repeat( "0", cantidadDigitos ) );
          string numeroVenta = ceros + cantidadDigitos.ToString();
          //0001

          numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos, cantidadDigitos);
          venta.NumeroDocumento = numeroVenta;

          await _dbContext.Venta.AddAsync(venta );
          await _dbContext.SaveChangesAsync();

          ventaGenerada = venta;
          transaction.Commit();
        }
        catch( Exception )
        {
          transaction.Rollback();
          throw;
        }

        return ventaGenerada;
      }
    }
  }
}
