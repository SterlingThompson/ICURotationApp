using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICURotationApp.Models
{
    public class Acceptance
    {
        public int? AcceptanceId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display (Name ="Golden Hour Number")]
        public int GoldenHourNumber { get; set; }

        [Display (Name ="Chief Complaint")]
        public string ChiefComplaint  { get; set; }

        [Display (Name ="Sending Facility")]
        public string SendingFacility { get; set; }

        [Display (Name ="Receiving Facility")]
        public string ReceivingFacility { get; set; }

        [Display (Name ="Initiated inside or outside of MedCom")]
        public string InitiationLocation { get; set; }

        [Display (Name ="Did you skip roation order? Why?")]
        public string SkipReason { get; set; }

    }
}
