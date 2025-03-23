using MVC03.DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage ="Name is Requied")]
        public string Name { get; set; }
        [Range (20 , 60)]
        public int? Age { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        public DateTime CreateAt { get; set; }

        [DisplayName("Department")]
        public int? DepartmentId { get; set; }

        public string? ImageName { get; set; }
        public IFormFile Image {  get; set; }

    }
}
