using System;
using System.Collections.Generic;

namespace Lignaris_Pizza_Backend.Models;

public partial class VentaDetalle
{
    public int IdVentaDetalle { get; set; }

    public int? IdVenta { get; set; }

    public int? IdReceta { get; set; }

    public decimal? Cantidad { get; set; }

    public virtual Recetum? IdRecetaNavigation { get; set; }

    public virtual Ventum? IdVentaNavigation { get; set; }
}
