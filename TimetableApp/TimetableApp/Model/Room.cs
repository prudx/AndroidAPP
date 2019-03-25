using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableApp.Model
{
    public class Room
    {
        [JsonProperty("room_Id")]
        public int Room_Id { get; set; }

        [JsonProperty("room_no")]
        public int Room_no { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
