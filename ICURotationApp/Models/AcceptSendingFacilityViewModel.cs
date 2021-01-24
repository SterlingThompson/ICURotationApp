using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICURotationApp.Models
{
    public class AcceptSendingFacilityViewModel
    {

        public List<Acceptance> Acceptances { get; set; }

        public SelectList SendingFacilities { get; set; }

        public SelectList ReceivingFacilities { get; set; }

        public string SFacilities { get; set; }

        public string RFacilities { get; set; }

    }
}
