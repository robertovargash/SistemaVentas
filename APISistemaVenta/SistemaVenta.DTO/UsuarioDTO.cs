namespace SistemaVenta.DTO
{
  public class UsuarioDTO
  {
    public int Idusuario { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Correro { get; set; }
    public int? IdRol { get; set; }
    public string? RolDescripcion { get; set; }
    public string? Clave { get; set; }
    public int EsActivo { get; set; }
  }
}
