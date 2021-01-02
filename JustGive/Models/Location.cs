using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        // many-to-one
        public virtual ICollection<Donation> Donations { get; set; }
        //many-to-one
        public virtual ICollection<Cause> Causes { get; set; }


    }
}