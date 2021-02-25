using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models
{
    //This class represents the information about the location of pupils
    public class Location
    {
        [Key]
        public int locId { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string incomeRange { get; set; }
        //A location can have many pupils
        public ICollection<Pupil> Pupils { get; set; }
    }

    //This class is used to transfer information about a location.
    //also known as a "Data Transfer Object"
    public class LocationDto
    {
        public int locId { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string incomeRange { get; set; }
    }
}