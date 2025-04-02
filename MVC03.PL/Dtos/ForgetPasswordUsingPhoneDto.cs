using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class ForgetPasswordUsingPhoneDto : ResetPasswordDto
    {
        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Invalid phone number format!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "OTP code is required!")]
        public string OTPCode { get; set; }
    }
}
