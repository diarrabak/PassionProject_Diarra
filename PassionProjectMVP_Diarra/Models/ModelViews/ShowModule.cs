using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class ShowModule
    {
        //Information about the module
        public ModuleDto module { get; set; }
        //Classe to which pupil belongs
        public ClasseDto classe { get; set; }
    }
}