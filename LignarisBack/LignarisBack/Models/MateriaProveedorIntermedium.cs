using System;
using System.Collections.Generic;

namespace Lignaris_Pizza_Backend.Models;

public partial class MateriaProveedorIntermedium
{
    public int IdMateriaProveedor { get; set; }

    public int IdMateriaPrima { get; set; }

    public int IdProveedor { get; set; }

    public virtual MateriaPrima IdMateriaPrimaNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
