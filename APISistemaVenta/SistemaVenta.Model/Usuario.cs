using System;
using System.Collections.Generic;

namespace SistemaVenta.Model;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Correro { get; set; }

    public int? IdRol { get; set; }

    public string? Clave { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
