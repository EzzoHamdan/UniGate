using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityGate.Models
{
    public class User
    {

        [Required]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Type { get; set; }
    }
}