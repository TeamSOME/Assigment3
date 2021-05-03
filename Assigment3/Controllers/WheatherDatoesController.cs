using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assigment3.Data;
using Assigment3.Models;

namespace Assigment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WheatherDatoesController : ControllerBase
    {
        private readonly Assigment3Context _context;

        public WheatherDatoesController(Assigment3Context context)
        {
            _context = context;
        }

        // GET: api/WheatherDatoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WheatherDato>>> GetWheatherDato()
        {
            return await _context.WheatherDato.ToListAsync();
        }

        // GET: api/WheatherDatoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WheatherDato>> GetWheatherDato(int id)
        {
            var wheatherDato = await _context.WheatherDato.FindAsync(id);

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
        public async Task<ActionResult<WheatherDato>> PostWheatherDato(WheatherDato wheatherDato)
        {
            _context.WheatherDato.Add(wheatherDato);
            await _context.SaveChangesAsync();

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
