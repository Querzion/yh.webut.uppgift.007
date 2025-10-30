using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Onatrix.ViewModels;

public class CallbackFormViewModel
{
    [Required(ErrorMessage = "Your Name is required.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Email address is required.")]
    [Display(Name = "Email address")]
    [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@\""]+(\.[^<>()\[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}(\.[0-9]{1,3}){3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
        ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = null!;
    
    [Required(ErrorMessage = "Please select an option.")]
    public string SelectedOption { get; set; } = null!;

    [BindNever]
    public IEnumerable<string> Options { get; set; } = [];
}