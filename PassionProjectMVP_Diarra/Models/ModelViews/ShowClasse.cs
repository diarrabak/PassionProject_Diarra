using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class ShowClasse
    {
        //Information about the module
        public ClasseDto classe { get; set; }

        //Information about all players on that team
        public IEnumerable<PupilDto> allPupils { get; set; }

        //Information about all sponsors for that team
        public IEnumerable<ModuleDto> allModules { get; set; }
    }
}