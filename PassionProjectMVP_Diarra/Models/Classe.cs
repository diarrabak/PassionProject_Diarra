using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models
{

    // This model represents a classe with its attibutes and the relationship with pupils and modules
    public class Classe
    {
        [Key]
        public int classId { get; set; }
        public string className { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        //A class is composed of many modules
        public ICollection<Pupil> Pupils { get; set; }

        //A class is taken by many pupils
        public ICollection<Module> Modules { get; set; }
    }
    //This class is used to transfer information about a Classe.
    //also known as a "Data Transfer Object"
    public class ClasseDto
    {
        public int classId { get; set; }
        public string className { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}