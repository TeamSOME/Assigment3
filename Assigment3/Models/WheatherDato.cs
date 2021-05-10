using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assigment3.Models
{
    public class WheatherDato
    {
        public int WheatherDatoID { get; set; }
        [Required]
        [Display(Name = "Place:")]
        public Location place { get; set; }
        [Required]
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "CurrentTime:")]
        public string CurrentTime => Date.ToString("HH:mm");
        [Required]
        [Display(Name = "Temperature in grad (C):")]
        public int TemperatureC { get; set; }
        [Required]
        [Display(Name = "Humidity in (%):")]
        public string Humidity { get; set; }
        [Required]
        [Display(Name = "Airpresser in (mBr):")]
        public string Airpresser { get; set; }

    }
  
}
    

