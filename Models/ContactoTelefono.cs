using System.ComponentModel.DataAnnotations;

namespace DirectorioMVC.Models
{
    public class ContactoTelefono
    {
        public int Id { get; set; }
        public Contacto Contacto { get; set; } = null!;
        public Telefono Telefono { get; set; } = null!;
    }
}


//namespace DirectorioMVC.Models
//{
//    public class ContactoTelefono
//    {
//        public Contacto Contacto { get; set; } = null!;
//        public List<Telefono> oTelefono { get; set; } = null!;
//    }
//}


