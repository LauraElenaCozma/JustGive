using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "The name must exist")]
        [MinLength(3, ErrorMessage = "The name of tag cannot be less than 3 characters!"),
            MaxLength(30, ErrorMessage = "The name of tag cannot be more than 30 characters!")]
        [RegularExpression(@"^[a-z0-9\s]*$", ErrorMessage = "The name of tag must contain only small letters, spaces and digits!")]
        public String Name { get; set; }

        // many-to-many
        public virtual ICollection<Donation> Donations { get; set; }
        //many-to-many 
        public virtual ICollection<Cause> Causes { get; set; }

    }
}