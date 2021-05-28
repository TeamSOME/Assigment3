using System;
using System.ComponentModel.DataAnnotations;

namespace Assigment3.Models
{
    public class WheatherDato
    {
        public int WheatherDatoID { get; set; }
        public Location place { get; set; }
        public DateTime Date { get; set; }
        public DateTime CurrentTime => Date; 
        public double TemperatureC { get; set; }
        public string Humidity { get; set; }
        public double Airpresser { get; set; }

    }
  
}
    

