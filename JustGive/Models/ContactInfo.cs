using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustGive.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        // one-to-one
        public virtual Cause Cause { get; set; }
    }
}