using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assigment3.Models
{
    public class Location
    {

        public string LocationID { get; set; }
        [Required]
        [Display(Name = "Name:")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Latitude:")]
        public string Latitude { get; set; }
        [Required]
        [Display(Name = "Longitude:")]
        public string Longitude { get; set; }
   //56.167873
   //10.203767
    }
}
