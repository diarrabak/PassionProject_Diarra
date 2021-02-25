using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProjectMVP_Diarra.Models.ModelViews
{
    public class ShowLocation
    {
        //Location of the pupil
        public LocationDto location { get; set; }
        //List of all pupils from that location
        public IEnumerable<PupilDto> pupils { get; set; }
    }
}