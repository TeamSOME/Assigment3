using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assigment3.Data;
using Assigment3.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Assigment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WheatherDatoesController : ControllerBase
    {
        private readonly Assigment3Context _context;
        private readonly IHubContext<WeatherHub> _hubContext;

        public WheatherDatoesController(Assigment3Context context, IHubContext<WeatherHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/WheatherDatoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetWheatherDato()
        {
            return await _context.WheatherDato.Include(l => l.place).ToListAsync();
        }

        //GET: BY DATE
        [HttpGet("/BYDATE")]

        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetByDateWeatherData(DateTime? date)
        {
            if (date == null) return NotFound();
            DateTime dateTime = (DateTime)date;
            return await _context.WheatherDato.Include(l => l.place).Where(l => l.Date.Date == dateTime.Date).ToListAsync();
        }

        //GET: LAST 3 OBSERVATIONS
        [HttpGet("/LAST3")]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetLatestWeatherData()
        {
            return await _context.WheatherDato.Include(l => l.place).OrderByDescending(l => l.Date).Take(3).ToListAsync();
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

        // GET: api/WheatherDatoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WheatherDato>> GetWheatherDato(int id)
        {
            var wheatherDato = await _context.WheatherDato
                .Include(l => l.place)
                .SingleOrDefaultAsync(l => l.WheatherDatoID == id);

            if (wheatherDato == null)
            {
                return NotFound();
            }

            return wheatherDato;
        }

        // PUT: api/WheatherDatoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWheatherDato(int id, WheatherDato wheatherDato)
        {
            if (id != wheatherDato.WheatherDatoID)
            {
                return BadRequest();
            }

            _context.Entry(wheatherDato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WheatherDatoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WheatherDatoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WheatherDato>> PostWheatherDato(WheatherDato wheatherDato)
        {
            _context.WheatherDato.Add(wheatherDato);
            await _context.SaveChangesAsync();

            if (_hubContext != null)
            {
                string JSON = JsonSerializer.Serialize(wheatherDato);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", JSON);
            }

            return CreatedAtAction("GetWheatherDato", new { id = wheatherDato.WheatherDatoID }, wheatherDato);
        }

        // DELETE: api/WheatherDatoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWheatherDato(int id)
        {
            var wheatherDato = await _context.WheatherDato.FindAsync(id);
            if (wheatherDato == null)
            {
                return NotFound();
            }

            _context.WheatherDato.Remove(wheatherDato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WheatherDatoExists(int id)
        {
            return _context.WheatherDato.Any(e => e.WheatherDatoID == id);
        }
    }
}
