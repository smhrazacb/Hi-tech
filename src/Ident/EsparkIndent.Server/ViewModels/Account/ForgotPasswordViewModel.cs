using System.ComponentModel.DataAnnotations;

namespace EsparkIndent.Server.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
