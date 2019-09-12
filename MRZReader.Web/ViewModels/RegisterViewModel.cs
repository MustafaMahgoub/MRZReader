using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MRZReader.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password{ get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm Passeord")]
        [Compare("Password", ErrorMessage= "Password and confirmation password did not match")]
        public string ConfirmPassword { get; set; }
    }
}
