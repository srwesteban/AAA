using DirectorioMVC.Models;
using DirectorioMVC.Models.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly DirectorioContext _context;

    public HomeController(DirectorioContext context)
    {
        _context = context;
    }

    // GET: Home
    public IActionResult Index()
    {
        var contactosConTelefonos = _context.Contactos.Include(c => c.Telefonos).ToList();
        return View(contactosConTelefonos);
    }

    // GET: Home/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contacto = await _context.Contactos.Include(c => c.Telefonos)
                                               .FirstOrDefaultAsync(m => m.Id == id);

        if (contacto == null)
        {
            return NotFound();
        }

        return View(contacto);
    }





    // GET: Home/Create
    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new ContactoTelefono
        {
            Contacto = new Contacto(),
            Telefono = new Telefono()
        };

        return View(viewModel);
    }

    // POST: Home/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ContactoTelefono viewModel)
    {
        if (ModelState.IsValid)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Guarda el nuevo contacto
                    _context.Contactos.Add(viewModel.Contacto);
                    _context.SaveChanges();

                    // Asigna el ID del nuevo contacto al tel�fono
                    viewModel.Telefono.ContactoId = viewModel.Contacto.Id;

                    // Guarda el nuevo tel�fono
                    _context.Telefonos.Add(viewModel.Telefono);
                    _context.SaveChanges();

                    transaction.Commit(); // Confirma la transacci�n si todo est� bien

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Deshace la transacci�n en caso de error
                    ModelState.AddModelError(string.Empty, "Error al guardar en la base de datos: " + ex.Message);

                    // Captura de errores en la consola
                    Debug.WriteLine("Error al guardar en la base de datos: " + ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        // Si el modelo no es v�lido o hay un error, vuelve a la vista con el modelo para mostrar los errores
        return View(viewModel);
    }













    // GET: Home/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contacto = await _context.Contactos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contacto == null)
        {
            return NotFound();
        }

        return View(contacto);
    }

    /// POST: Home/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contacto = await _context.Contactos.FindAsync(id);

        if (contacto != null)
        {
            // Elimina todos los tel�fonos asociados al contacto
            var telefonos = _context.Telefonos.Where(t => t.ContactoId == id);
            _context.Telefonos.RemoveRange(telefonos);

            // Elimina el contacto
            _context.Contactos.Remove(contacto);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }





    // GET: Home/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contacto = await _context.Contactos.Include(c => c.Telefonos)
                                               .FirstOrDefaultAsync(m => m.Id == id);

        if (contacto == null)
        {
            return NotFound();
        }

        var viewModel = new ContactoTelefono
        {
            Contacto = contacto,
            Telefono = contacto.Telefonos.FirstOrDefault() ?? new Telefono()
        };

        return View(viewModel);
    }

    private bool ContactoExists(int id)
    {
        return _context.Contactos.Any(c => c.Id == id);
    }


    // POST: Home/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, ContactoTelefono viewModel)
    {
        // Verifica si el ID proporcionado coincide con el ID del modelo
        if (id != viewModel.Contacto.Id)
        {
            return NotFound(); // Retorna un error 404 si no coincide
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Actualiza el contacto en la base de datos
                _context.Contactos.Update(viewModel.Contacto);

                // Verifica si el tel�fono ya existe para este contacto
                var existingTelefono = _context.Telefonos.FirstOrDefault(t => t.ContactoId == viewModel.Contacto.Id);

                if (existingTelefono != null)
                {
                    // Si el tel�fono existe, actualiza sus propiedades
                    existingTelefono.NumeroTelefono = viewModel.Telefono.NumeroTelefono;
                    existingTelefono.TipoTelefono = viewModel.Telefono.TipoTelefono;
                    _context.Telefonos.Update(existingTelefono);
                }
                else
                {
                    // Si el tel�fono no existe, agr�galo
                    _context.Telefonos.Add(viewModel.Telefono);
                }

                // Guarda los cambios en la base de datos
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactoExists(id))
                {
                    return NotFound(); // Retorna un error 404 si el contacto no existe
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Si el modelo no es v�lido, vuelve a la vista con el modelo para mostrar los errores
        return View(viewModel);
    }


}
