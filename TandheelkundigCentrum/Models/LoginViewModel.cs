using System.ComponentModel.DataAnnotations;

namespace TandheelkundigCentrum.Models;

public class LoginViewModel
{
    [Display(Name = "Email Adres")]
    [Required(ErrorMessage = "Email mag niet leeg zijn.")]
    [EmailAddress(ErrorMessage = "Dat is geen geldig email adress.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Wachtwoord mag niet leeg zijn.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}