using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimetableAPI.Models;

namespace TimetableAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly TimetableAPIContext _context;

        public TimetablesController(TimetableAPIContext context)
        {
            _context = context;
        }

        // GET: api/Timetables
        [HttpGet]
        public IEnumerable<Timetable> GetTimetable()
        {
            return _context.Timetable;
        }

        [HttpGet("rooms")]
        public IEnumerable<Timetable> GetRooms()
        {
            return _context.Timetable.Include(p => p.Room).Include(c => c.Calendar).ToList();
        }


        [HttpGet("{roomNo}")]
        public IEnumerable<Timetable> GetByRoomNo(int? roomNo)
        {
            
            var room = _context.Rooms;
            
            var rooms = _context.Timetable.Include(t => t.Room).ThenInclude(r => r.Room_no == roomNo).ToList();
            return rooms;

        }
        public Timetable Timetable { get; set; }

        [HttpGet("{roomIn}")]
        public async Task<IActionResult> OnGetAsync(int? roomIn)
        {
            if (roomIn == null)
            {
                return NotFound();
            }

            Timetable = await _context.Timetable.
                 FirstOrDefaultAsync(m => m.Room.Room_no == roomIn);

            if (Timetable == null)
            {
                return NotFound();
            }
            return Ok(Timetable);
        }


        // GET: api/Timetables/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimetable([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timetable = await _context.Timetable.FindAsync(id);

            if (timetable == null)
            {
                return NotFound();
            }

            return Ok(timetable);
        }

        // PUT: api/Timetables/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable([FromRoute] int id, [FromBody] Timetable timetable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timetable.Timetable_Id)
            {
                return BadRequest();
            }

            _context.Entry(timetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(id))
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

        // POST: api/Timetables
        [HttpPost]
        public async Task<IActionResult> PostTimetable([FromBody] Timetable timetable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Timetable.Add(timetable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimetable", new { id = timetable.Timetable_Id }, timetable);
        }

        // DELETE: api/Timetables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timetable = await _context.Timetable.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }

            _context.Timetable.Remove(timetable);
            await _context.SaveChangesAsync();

            return Ok(timetable);
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetable.Any(e => e.Timetable_Id == id);
        }
    }
}