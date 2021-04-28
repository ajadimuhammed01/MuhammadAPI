using System;
using System.ComponentModel.DataAnnotations;

namespace techHowdy.API.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name="User Name")]
        public string UserName {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }
}