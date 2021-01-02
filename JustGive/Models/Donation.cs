using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Models
{
    public class Donation
    {
        public int DonationId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        //one-to-many
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        // many-to-many
        public virtual ICollection<Tag> Tags { get; set; }
        //one-to-many
        //public int CauseId { get; set; }
        public virtual Cause Cause { get; set; }

        //list for choosing the location
        [NotMapped]
        public IEnumerable<SelectListItem> LocationList { get; set; }
        //list for choosing the cause
        [NotMapped]
        public IEnumerable<SelectListItem> CauseList { get; set; }
        //checkbox for many-to-many-relationship
        [NotMapped]
        public List<TagDonationViewModel> TagList { get; set; }


    }
}