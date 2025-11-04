using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Onatrix.ViewModels
{
    public class QuestionFormViewModel
    {
        [Required(ErrorMessage = "Your Name is required.")]
        [Display(Name = "Name", Prompt = "Enter your full name")]
        public string Name { get; set; } = null!;
    
        [Required(ErrorMessage = "Email address is required.")]
        [Display(Name = "Email address", Prompt = "Enter your email address")]
        [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@\""]+(\.[^<>()\[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}(\.[0-9]{1,3}){3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;
    
        [Required(ErrorMessage = "A Question is required.")]
        [Display(Name = "Question", Prompt = "Write your question here")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Your question must be between 10 and 1000 characters long.")]
        public string Question { get; set; } = null!;
    }
}
