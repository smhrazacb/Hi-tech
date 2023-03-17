using System.ComponentModel.DataAnnotations;

namespace EsparkIndent.Server.ViewModels.Account;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
