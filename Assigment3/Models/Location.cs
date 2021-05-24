
using System.ComponentModel.DataAnnotations;


namespace Assigment3.Models
{
    public class Location
    {

        public int LocationID { get; set; }
        [Required]
        [Display(Name = "Name:")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Latitude:")]
        public double Latitude { get; set; }
        [Required]
        [Display(Name = "Longitude:")]
        public double Longitude { get; set; }
   //56.167873
   //10.203767
    }
}
