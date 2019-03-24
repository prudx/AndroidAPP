using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TimetableAPI.Models
{
    public class TimetableAPIContext : DbContext
    {
        public TimetableAPIContext (DbContextOptions<TimetableAPIContext> options)
            : base(options)
        {
        }

        public DbSet<TimetableAPI.Models.Timetable> Timetable { get; set; }
        public DbSet<TimetableAPI.Models.Room> Rooms { get; set; }
    }
}
