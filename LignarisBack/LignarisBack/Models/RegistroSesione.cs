using LignarisBack.Models;
using System;
using System.Collections.Generic;

namespace Lignaris_Pizza_Backend.Models;

public partial class RegistroSesione
{
    public int IdRegistro { get; set; }

    public DateTime? FechaHoraAccion { get; set; }

    public int? EstatusSesion { get; set; }

    public string? IdUsuario { get; set; }

    public virtual AppUser? IdUsuarioNavigation { get; set; }
}
