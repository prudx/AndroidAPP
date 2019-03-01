using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableAPI.Models
{
    public class Room
    {
        [Key]
        public int Room_Id { get; set; }
        public int Room_no { get; set; }
        public string Description { get; set; }
    }
}
