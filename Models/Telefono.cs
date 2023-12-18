using System;
using System.Collections.Generic;

namespace DirectorioMVC.Models;

public partial class Telefono
{
    public int Id { get; set; }

    public int? ContactoId { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public string? TipoTelefono { get; set; }

    public virtual Contacto? Contacto { get; set; }
}
