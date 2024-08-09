using System;
using System.Collections.Generic;

namespace Lignaris_Pizza_Backend.Models;

public partial class Produccion
{
    public int IdProduccion { get; set; }

    public int? IdVenta { get; set; }

    public int? IdEmpleado { get; set; }

    public DateTime? FechaProduccion { get; set; }

    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    public virtual Ventum? IdVentaNavigation { get; set; }
}
