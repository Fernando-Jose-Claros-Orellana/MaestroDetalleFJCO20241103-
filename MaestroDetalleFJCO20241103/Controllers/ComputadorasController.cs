using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaestroDetalleFJCO20241103.Models;

namespace MaestroDetalleFJCO20241103.Controllers
{
    public class ComputadorasController : Controller
    {
        private readonly MaestroDetalleFJCO20241103DBContext _context;

        public ComputadorasController(MaestroDetalleFJCO20241103DBContext context)
        {
            _context = context;
        }

        // GET: Computadoras
        public async Task<IActionResult> Index()
        {
              return _context.Computadoras != null ? 
                          View(await _context.Computadoras.ToListAsync()) :
                          Problem("Entity set 'MaestroDetalleFJCO20241103DBContext.Computadoras'  is null.");
        }

        // GET: Computadoras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Computadoras == null)
            {
                return NotFound();
            }

            var computadora = await _context.Computadoras
                .Include(s=> s.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computadora == null)
            {
                return NotFound();
            }
            ViewBag.EnableComboBox = false; // Deshabilitar el combo box en la vista Details
            ViewBag.Accion = "Details";
            return View(computadora);
        }

        // GET: Computadoras/Create
        public IActionResult Create()
        {
            var computadora = new Computadora();
            computadora.Nombre = "";
            computadora.Marca = "";
            computadora.Precio = 0;
            computadora.Componente = new List<Componente>();
            computadora.Componente.Add(new Componente
            {
                Precio = 1
            });
            ViewBag.EnableComboBox = true; // Habilitar el combo box en la vista Create
            ViewBag.Accion = "Create";
            return View(computadora);
        }

        // POST: Computadoras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Marca,Precio,Componente")] Computadora computadora)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computadora);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computadora);
        }
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,Nombre,Marca,Precio,Componente")] Computadora computadora, string accion)
        {
            computadora.Componente.Add(new Componente { Precio = 1 });
            ViewBag.Accion = accion;
            ViewBag.EnableComboBox = true; // Habilitar el combo box en la vista Create
            return View(accion, computadora);
        }

        [HttpPost]
        public ActionResult EliminarDetalles([Bind("Id,Nombre,Marca,Precio,Componente")] Computadora computadora,
               int index, string accion)
        {
            var det = computadora.Componente[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                computadora.Componente.RemoveAt(index);
            }
            ViewBag.EnableComboBox = true; // Habilitar el combo box en la vista Create
            ViewBag.Accion = accion;
            return View(accion, computadora);
        }

        // GET: Computadoras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Computadoras == null)
            {
                return NotFound();
            }

            var computadora = await _context.Computadoras
                  .Include(s => s.Componente)
                  .FirstAsync(s => s.Id == id);
            if (computadora == null)
            {
                return NotFound();
            }
            ViewBag.EnableComboBox = true; // Habilitar el combo box en la vista Create
            ViewBag.Accion = "Edit";
            return View(computadora);
        }

        // POST: Computadoras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Marca,Precio,Componente")] Computadora computadora)
        {
            if (id != computadora.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener los datos de la base de datos que van a ser modificados
                    var compUpdate = await _context.Computadoras
                            .Include(s => s.Componente)
                            .FirstAsync(s => s.Id == computadora.Id);
                    compUpdate.Precio = computadora.Precio;
                    compUpdate.Nombre = computadora.Nombre;
                    compUpdate.Marca = computadora.Marca;
                    // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                    var detNew = computadora.Componente.Where(s => s.Id == 0);
                    foreach (var d in detNew)
                    {
                        compUpdate.Componente.Add(d);
                    }
                    // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                    var detUpdate = computadora.Componente.Where(s => s.Id > 0);
                    foreach (var d in detUpdate)
                    {
                        var det = compUpdate.Componente.FirstOrDefault(s => s.Id == d.Id);
                        det.Nombre = d.Nombre;
                        det.Tipo = d.Tipo;
                        det.Precio = d.Precio;
                    }
                    // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                    var delDet = computadora.Componente.Where(s => s.Id < 0).ToList();
                    if (delDet != null && delDet.Count > 0)
                    {
                        foreach (var d in delDet)
                        {
                            d.Id = d.Id * -1;
                            var det = compUpdate.Componente.FirstOrDefault(s => s.Id == d.Id);
                            _context.Remove(det);
                            // facturaUpdate.DetFacturaVenta.Remove(det);
                        }
                    }

                     _context.Update(compUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputadoraExists(computadora.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(computadora);
        }

        // GET: Computadoras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Computadoras == null)
            {
                return NotFound();
            }

            var computadora = await _context.Computadoras
                .Include(s => s.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computadora == null)
            {
                return NotFound();
            }
            ViewBag.EnableComboBox = false; // Deshabilitar el combo box en la vista Details
            ViewBag.Accion = "Delete";
            return View(computadora);
        }

        // POST: Computadoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Computadoras == null)
            {
                return Problem("Entity set 'MaestroDetalleFJCO20241103DBContext.Computadoras'  is null.");
            }
            var computadora = await _context.Computadoras
                .FindAsync(id);
            if (computadora != null)
            {
                _context.Computadoras.Remove(computadora);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputadoraExists(int id)
        {
          return (_context.Computadoras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
