using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assigment3.Models
{
    public class WheatherDato
    {
        public int WheatherDatoID { get; set; }
       
        public Location place { get; set; }
        public DateTime Date { get; set; }

        public string CurrentTime => Date.ToString("HH:mm");
        public int TemperatureC { get; set; }

        public string Humidity { get; set; }
        public string Airpresser { get; set; }

    }
  
}
    

