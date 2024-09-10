using System.Diagnostics;

namespace WebAPI5.Entities
{
    public class Enrollment
    {
        public long EnrollmentID { get; set; }
        public long StudentID { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int Credits { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Grade { get; set; }
        public virtual Student Students { get; set; }
        // FOREIGN KEY(StudentID) REFERENCES Student(StudentID)
    }
}
