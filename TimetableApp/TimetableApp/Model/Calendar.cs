using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableApp.Model
{
    public class Calendar
    {
        [JsonProperty("calendar_Id")]
        public int Calendar_Id { get; set; }

        [JsonProperty("calendarDate")]
        public DateTime CalendarDate { get; set; }

        [JsonProperty("dayofWeek")]
        public string DayofWeek { get; set; }

        [JsonProperty("weekNumber")]
        public int WeekNumber { get; set; }

    }
}
