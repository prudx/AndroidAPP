using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableAPI.Models
{
    public class Timetable
    {
        [Key]
        public int Timetable_Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Room Room { get; set; }
        public Calendar Calendar { get; set; }

        [ForeignKey("Calendar_Id")]
        public int Calendar_Id { get; set; }

        [ForeignKey("Room_Id")]
        public int Room_Id { get; set; }

    }
}
