using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models
{
    //This class represents the information about a module
    public class Module
    {
        [Key]
        public int modId { get; set; }
        public string moduleName { get; set; }
        public string description { get; set; }
        public string delivery { get; set; }
        public Decimal fees { get; set; }

        //A module belongs to a class
        [ForeignKey("Classe")]
        public int classId { get; set; }
        public virtual Classe Classe { get; set; }
    }

    //This class is used to transfer information about a module.
    //also known as a "Data Transfer Object"
    public class ModuleDto
    {
        public int modId { get; set; }
        public string moduleName { get; set; }
        public string description { get; set; }
        public string delivery { get; set; }
        public Decimal fees { get; set; }
        public int classId { get; set; }
    }
}