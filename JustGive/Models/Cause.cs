using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Models
{
    public class Cause
    {
        public int CauseId { get; set; }

        [Required(ErrorMessage = "The title must exist")]
        [MinLength(3, ErrorMessage = "Title cannot be less than 3 characters!"),
            MaxLength(70, ErrorMessage = "Title cannot be more than 70 characters!")]
        [RegularExpression(@"^[A-Z].*$", ErrorMessage = "The title must start with an uppercase!")]
        public String Title { get; set; }

        [Required(ErrorMessage = "The description must exist")]
        [MinLength(5, ErrorMessage = "Description cannot be less than 5 characters!"),
            MaxLength(200, ErrorMessage = "Description cannot be more than 200 characters!")]
        public String Description { get; set; }

        //one-to-many
        [Required(ErrorMessage = "The location must be selected")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        // many-to-many
        public virtual ICollection<Tag> Tags { get; set; }
        //one-to-one
        [Required(ErrorMessage ="You must insert a contact info!")]
        public virtual ContactInfo ContactInfo { get; set; }
        // many-to-one
        public virtual ICollection<Donation> Donations { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //list for choosing the location
        [NotMapped]
        public IEnumerable<SelectListItem> LocationList { get; set; }
        
        //checkbox for many-to-many-relationship
        [NotMapped]
        public List<TagDonationViewModel> TagList { get; set; }

    }
}