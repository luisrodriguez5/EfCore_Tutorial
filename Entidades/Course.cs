using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EfCore_Tutorial.Entidades
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        //Aplicando relaciones 
        public int studentId { get; set; }
        [ForeignKey("studentId")]
        public virtual Student Student { get; set; }

        public Course()
        {
            CourseId = 0;
            CourseName = string.Empty;
            studentId = 0;
        }
        
    }
}