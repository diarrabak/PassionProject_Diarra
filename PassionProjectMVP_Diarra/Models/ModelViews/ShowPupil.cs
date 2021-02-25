using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class ShowPupil
    {
        //Information about the pupil
        public PupilDto pupil { get; set; }
        //Classe to which pupil belongs 
        public ClasseDto classe { get; set; }
        //Location of the pupil
        public LocationDto location { get; set; }
    }
}