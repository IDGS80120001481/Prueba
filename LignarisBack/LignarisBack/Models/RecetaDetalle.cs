using System;
using System.Collections.Generic;

namespace Lignaris_Pizza_Backend.Models;

public partial class RecetaDetalle
{
    public int IdRecetaDetalle { get; set; }

    public int? IdReceta { get; set; }

    public int IdMateriaPrima { get; set; }

    public decimal? Cantidad { get; set; }

    public virtual MateriaPrima? IdMateriaPrimaNavigation { get; set; }

    public virtual Recetum? IdRecetaNavigation { get; set; }
}
