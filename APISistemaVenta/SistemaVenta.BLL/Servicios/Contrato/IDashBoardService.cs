﻿using SistemaVenta.DTO;

namespace SistemaVenta.BLL.Servicios.Contrato
{
  public interface IDashBoardService
  {
    Task<DashboardDTo> Resumen();
  }
}
