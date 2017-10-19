using System;
using System.ComponentModel.DataAnnotations;

namespace InteticsTestApp.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"^[A-Za-z''-'\s]*$", ErrorMessage = "Wrong Username")]
        //[RegularExpression(@"[A-Za-z]", ErrorMessage = "Некорректный адрес")]
        public string Username { get; set; }


        [Required]  
        [RegularExpression(@"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$", ErrorMessage = "Wrong Email address")]
        public string Email { get; set; }

        
        public string ProfileName { get; set; }
        [Required]
        //[DataType(DataType.Password)]
        [StringLength(30, MinimumLength= 6, ErrorMessage = "Wrong count of symbol")]
        [RegularExpression(@"^[^/\\()~!@#$%^&*]*$", ErrorMessage = "Not valid symbol in password ")]

        public string Password { get; set; }


        [Required]
        [Compare("Password", ErrorMessage = "passwords not match")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Wrong count of symbol")]
        [RegularExpression(@"^[^/\\()~!@#$%^&*]*$", ErrorMessage = "Not valid symbol in password ")]


        public string PasswordConfirm { get; set; }
    }
}