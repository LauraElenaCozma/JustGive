using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public String Name { get; set; }

        // many-to-many
        public virtual ICollection<Donation> Donations { get; set; }
        //many-to-many 
        public virtual ICollection<Cause> Causes { get; set; }

    }
}