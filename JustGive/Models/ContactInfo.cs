using JustGive.Models.MyValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name must exist")]
        [MinLength(3, ErrorMessage = "The name cannot be less than 3 characters!"),
            MaxLength(40, ErrorMessage = "The name cannot be more than 40 characters!")]
        [RegularExpression(@"^[A-Z][a-zA-Z-\s]*$", ErrorMessage = "The name of contact must contain only letters and start with a capital letter!")]
        public String Name { get; set; }

        [Required(ErrorMessage = "The phone number must exist")]
        [RegularExpression(@"^07(\d{8})$", ErrorMessage = "This is not a valid phone number!")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "The birthday must be set")]
        [BirthDateValidator]
        public DateTime BirthDate { get; set; }
        // one-to-one
        public virtual Cause Cause { get; set; }
    }
}