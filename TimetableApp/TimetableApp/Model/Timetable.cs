using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableApp.Model
{
    public class Timetable
    {
        [JsonProperty("timetable_Id")]
        public int Timetable_Id { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("room")]
        public Room Room { get; set; }

        [JsonProperty("calendar")]
        public Calendar Calendar { get; set; }

        [JsonProperty("calendar_Id")]
        public int Calendar_Id { get; set; }

        [JsonProperty("room_Id")]
        public int Room_Id { get; set; }
    }
}
