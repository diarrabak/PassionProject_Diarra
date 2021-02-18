using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class EditPupil
    {
        //Information about the pupil
        public PupilDto pupil { get; set; }
        //Needed for a dropdownlist which presents the pupil with a choice of classes 
        public IEnumerable<ClasseDto> allClasses { get; set; }
    }
}