using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebAPI5.Entities
{
    public class Student
    {

        public long StudentID { get; set; }
        [Required(ErrorMessage = "Fisrt name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage= "Last name is required")]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter you gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

}
