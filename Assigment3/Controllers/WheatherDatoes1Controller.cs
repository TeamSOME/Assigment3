using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assigment3.Data;
using Assigment3.Models;

namespace Assigment3.Controllers
{
    public class WheatherDatoes1Controller : Controller
    {
        private readonly Assigment3Context _context;

        public WheatherDatoes1Controller(Assigment3Context context)
        {
            _context = context;
        }

        // GET: WheatherDatoes1
        public async Task<IActionResult> Index()
        {
            return View(await _context.WheatherDato.ToListAsync());
        }

        // GET: WheatherDatoes1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wheatherDato = await _context.WheatherDato
                .FirstOrDefaultAsync(m => m.WheatherDatoID == id);
            if (wheatherDato == null)
            {
                return NotFound();
            }

            return View(wheatherDato);
        }

        // GET: WheatherDatoes1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WheatherDatoes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WheatherDatoID,Date,TemperatureC,Humidity,Airpresser")] WheatherDato wheatherDato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wheatherDato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wheatherDato);
        }

        // GET: WheatherDatoes1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wheatherDato = await _context.WheatherDato.FindAsync(id);
            if (wheatherDato == null)
            {
                return NotFound();
            }
            return View(wheatherDato);
        }

        // POST: WheatherDatoes1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WheatherDatoID,Date,TemperatureC,Humidity,Airpresser")] WheatherDato wheatherDato)
        {
            if (id != wheatherDato.WheatherDatoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wheatherDato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WheatherDatoExists(wheatherDato.WheatherDatoID))
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
            return View(wheatherDato);
        }

        // GET: WheatherDatoes1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wheatherDato = await _context.WheatherDato
                .FirstOrDefaultAsync(m => m.WheatherDatoID == id);
            if (wheatherDato == null)
            {
                return NotFound();
            }

            return View(wheatherDato);
        }

        // POST: WheatherDatoes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wheatherDato = await _context.WheatherDato.FindAsync(id);
            _context.WheatherDato.Remove(wheatherDato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WheatherDatoExists(int id)
        {
            return _context.WheatherDato.Any(e => e.WheatherDatoID == id);
        }
    }
}
