using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICURotationApp.Models
{
    public partial class Denials
    {
        [Key]
        public int DenialId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display (Name = "Golden Hour Number")]
        public int GoldenHourNumber { get; set; }

        [Display (Name = "Chief Complaint")]
        public string ChiefComplaint { get; set; }

        [Display (Name = "Sending Facility")]
        public string SendingFacility { get; set; }

        [Display (Name = "Receiving Facility")]
        public string ReceivingFacility { get; set; }

        [Display (Name = "Denial Reason")]
        public string DenialReason { get; set; }

    }
}
