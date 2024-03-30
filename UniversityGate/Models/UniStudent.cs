using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityGate.Models
{
    [MetadataType(typeof(MyDataAnnotation))]
    public partial class Student
    {
        public class MyDataAnnotation
        {
            
            [Required]
            public string first_name { get; set; }
            [Required]
            public string last_name { get; set; }

        }

    }
}