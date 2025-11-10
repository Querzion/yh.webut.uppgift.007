using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onatrix.ViewModels
{
    public class HelpFormViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [Display(Name = "E-mail address", Prompt = "Enter your email address")]
        [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@\""]+(\.[^<>()\[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}(\.[0-9]{1,3}){3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;
    }
}