using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICURotationApp.Models
{
    public class DenialViewModel
    {
        public List<Denials> Denials { get; set; }

        public SelectList SendingFacilities { get; set; }

        public SelectList ReceivingFacilities { get; set; }

        public string DeniedSendingFacilities { get; set; }

        public string DeniedReceivingFacilites { get; set; }

    }
}
