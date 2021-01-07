using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        [Required(ErrorMessage = "The city must exist")]
        [MinLength(2, ErrorMessage = "The city cannot be less than 2 characters!"),
            MaxLength(40, ErrorMessage = "The city cannot be more than 40 characters!")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "The city must start with an uppercase!")]
        public String City { get; set; }

        [Required(ErrorMessage = "The country must exist")]
        [MinLength(2, ErrorMessage = "The country cannot be less than 2 characters!"),
            MaxLength(40, ErrorMessage = "The country cannot be more than 40 characters!")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "The country must start with an uppercase!")]
        public String Country { get; set; }
        // many-to-one
        public virtual ICollection<Donation> Donations { get; set; }
        //many-to-one
        public virtual ICollection<Cause> Causes { get; set; }


    }
}