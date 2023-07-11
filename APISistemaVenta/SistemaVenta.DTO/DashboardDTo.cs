namespace SistemaVenta.DTO
{
  public class DashboardDTo
  {
    public int TotalVentas { get; set; }
    public string? TotalIngresos { get; set; }  
    public List<VentaSemanaDTO> VentasUltimaSemana { get; set;}
  }
}
