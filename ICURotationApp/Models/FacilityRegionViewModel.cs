using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICURotationApp.Models
{
    public class FacilityRegionViewModel
    {
        public List<FacilityList> Facilities { get; set; }

       

        public List<Denials> Denials { get; set; }

        public SelectList Regions { get; set; }
        public string FacilityRegion { get; set; }
        public string SearchString { get; set; }


        
      

       

        public SelectList ReceivingFacilities { get; set; }

        



        public SelectList DenialSendingFacilities { get; set; }

        public string DeniedSFacilities { get; set; }

        public SelectList DenialReceivingFacilities { get; set; }

        public string DeniedRFacilities { get; set; }

    }
}
