using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maio11_Best.Models
{
    public class MyPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime? Birthdate { get; set; }
        public string TeamName { get; set; }
        public string PhotoPath { get; set; }

    }
}