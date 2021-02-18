using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models
{
    //This class represents the information about a pupil
    public class Pupil
    {
        [Key]
        public int pId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }


        //A pupil belongs to a class
        [ForeignKey("Classe")]
        public int classId { get; set; }
        public virtual Classe Classe { get; set; }
        
        //A pupil has a location
        [ForeignKey("Location")]
        public int locId { get; set; }
        public virtual Location Location { get; set; }
    }

    //This class is used to transfer information about a pupil.
    //also known as a "Data Transfer Object"
    public class PupilDto
    {
        public int pId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }

        //A pupil belongs to a class
        public int classId { get; set; }

        //A pupil has a location
        public int locId { get; set; }
    }
}