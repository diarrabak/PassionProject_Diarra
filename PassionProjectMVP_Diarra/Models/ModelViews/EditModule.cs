using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class EditModule
    {
        //Information about the module
        public ModuleDto module { get; set; }
        //Needed for a dropdownlist which presents the module with a choice of classes 
        public IEnumerable<ClasseDto> allClasses { get; set; }
    }
}