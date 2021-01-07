using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class CauseContactViewModel
    {
        public Cause Cause { get; set; }
        [Required]
        public ContactInfo ContactInfo { get; set; }

    }
}