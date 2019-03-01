using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableAPI.Models
{
    public class Calendar
    {
        [Key]
        public int Claendar_Id { get; set; }
        public DateTime CalendarDate { get; set; }
        public string DayofWeek { get; set; }
        public int WeekNumber { get; set; }

    }
}
