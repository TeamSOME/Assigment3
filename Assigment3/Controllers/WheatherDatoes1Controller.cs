using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assigment3.Data;
using Assigment3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Assigment3.Controllers
{
    public class WheatherDatoes1Controller : Controller
    {
        private readonly Assigment3Context _context;
        private readonly IHubContext<WeatherHub> _hubContext;

        public WheatherDatoes1Controller(Assigment3Context context, IHubContext<WeatherHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: WheatherDatoes1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetWeatherData()
        {
            return await _context.WheatherDato.Include(l=>l.place).ToListAsync();
        }

        //GET: 3 DAYS
        [HttpGet("/3DAYS")]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetLatestWeatherData()
        {
            return await _context.WheatherDato.Include(l => l.place).OrderByDescending(l => l.Date).Take(3).ToListAsync();
        }
        //GET: BY DATE
        [HttpGet("/BYDATE")]

        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetByDateWeatherData(DateTime? date)
        {
            if (date == null) return NotFound();
            DateTime dateTime = (DateTime) date;
            return await _context.WheatherDato.Include(l => l.place).Where(l => l.Date.Date == dateTime.Date).ToListAsync();
        }

        //GET: INTERVAL
        [HttpGet("/INTERVAL")]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetIntervalWeatherData(DateTime? dateStart, DateTime? dateStop)
        {
            var intervalObservation = await _context.WheatherDato.ToListAsync();
            var q = new List<WheatherDato>();


            foreach (var interval in intervalObservation)
            {
                if (interval.CurrentTime >= dateStart && interval.CurrentTime <= dateStop)
                {
                    q.Add(interval);
                }
            }

            return q;
         }

        // GET: WheatherDatoes1/Details/5
        public async Task<IActionResult> Details(int? id) //NULLABLE?
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
        
        //POST WEATHER OBSERVATION IF AUTHENTICATED
        [HttpPost("/POST DATA")]
        [Authorize(AuthenticationSchemes = "JwtBearer")]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> PostWeatherData(WheatherDato wheatherDato)
        {

            _context.WheatherDato.Add(wheatherDato);
            await _context.SaveChangesAsync();

            if (_hubContext != null)
            {
                string JSON = JsonSerializer.Serialize(wheatherDato);
                await _hubContext.Clients.All.SendAsync("Yeet ya data", JSON);
            }

            return CreatedAtAction("GetWeatherObservation", new { id = wheatherDato.WheatherDatoID }, wheatherDato);
        }






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
        public async Task<IActionResult> Delete(int? id) //NULLABLE??
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
