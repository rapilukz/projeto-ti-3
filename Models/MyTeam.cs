using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maio11_Best.Models
{
    public class MyTeam
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int? FoundationYear { get; set; }
        public string Country { get; set; }
        public string PhotoPath { get; set; }
    }
}