using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICURotationApp.Models
{
    public partial class FacilityList
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }

        [Display (Name = "Next in Rotation")]
        public bool NextInRotation { get; set; }

        [Display (Name = "Skips")]
        public int NumberOfSkips { get; set; }
    }
}
