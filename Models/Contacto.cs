using System;
using System.Collections.Generic;

namespace DirectorioMVC.Models;

public partial class Contacto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
}
