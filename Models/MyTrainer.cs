using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maio11_Best.Models
{
    public class MyTrainer
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string CoachingLicense { get; set; }
        public string PhotoPath { get; set; }
        public string TeamName { get; set; }
    }
}