using System;
using System.ComponentModel.DataAnnotations;

namespace Assigment3.Models
{
    public class WheatherDato
    {
        public int WheatherDatoID { get; set; }
        [Display(Name = "Place:")]
        public Location place { get; set; }
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
        [Display(Name = "CurrentTime:")]
        public DateTime CurrentTime => Date; 
        [Display(Name = "Temperature in grad (C):")]
        public double TemperatureC { get; set; }
        [Display(Name = "Humidity in (%):")]
        public string Humidity { get; set; }
        [Display(Name = "Airpresser in (mBr):")]
        public double Airpresser { get; set; }

    }
  
}
    

