//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UniversityGate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Grades = new HashSet<Grade>();
            this.EnrolledCourses = new HashSet<EnrolledCours>();
        }
    
        public int student_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<System.DateTime> date_of_birth { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> enrollment_date { get; set; }
        public string studentID { get; set; }
        public string studentPassword { get; set; }
        public Nullable<int> UserRole { get; set; }
        public string ImagePath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Grade> Grades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrolledCours> EnrolledCourses { get; set; }
        public virtual Role Role { get; set; }
    }
}
