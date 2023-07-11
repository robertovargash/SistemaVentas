﻿using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.Model;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.DAL.Repositorios
{
  public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
  {
    private readonly DbventaContext _dbContext;

    public GenericRepository( DbventaContext dbContext )
    {
      _dbContext = dbContext;
    }

    public async Task<TModelo> Obtener( Expression<Func<TModelo, bool>> filtro )
    {
      try
      {
        TModelo modelo = await _dbContext.Set<TModelo>().FirstOrDefaultAsync(filtro);
        return modelo;
      }
      catch( Exception )
      {

        throw;
      }
    }

    public async Task<TModelo> Crear( TModelo modelo )
    {
      try
      {
        _dbContext.Set<TModelo>().Add( modelo);
        await _dbContext.SaveChangesAsync();
        return modelo;
      }
      catch( Exception )
      {

        throw;
      }
    }

    public async Task<bool> Editar( TModelo modelo )
    {
      try
      {
        _dbContext.Set<TModelo>().Update( modelo);
        await _dbContext.SaveChangesAsync( );
        return true;
      }
      catch( Exception )
      {

        throw;
      }
    }

    public async Task<bool> Eliminar( TModelo modelo )
    {
      try
      {
        _dbContext.Set<TModelo>().Remove( modelo );
        await _dbContext.SaveChangesAsync();
        return true;
      }
      catch( Exception )
      {

        throw;
      }
    }


    public async Task<IQueryable<TModelo>> Consultar( Expression<Func<TModelo, bool>> filtro = null )
    {
      try
      {
        IQueryable <TModelo> queryModelo = filtro == null ? _dbContext.Set<TModelo>() : _dbContext.Set<TModelo>().Where(filtro);
        return queryModelo;
      }
      catch( Exception )
      {

        throw;
      }
    }
  }
}
