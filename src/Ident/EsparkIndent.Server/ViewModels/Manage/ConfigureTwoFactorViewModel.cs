using Microsoft.AspNetCore.Mvc.Rendering;

namespace EsparkIndent.Server.ViewModels.Manage;

public class ConfigureTwoFactorViewModel
{
    public string SelectedProvider { get; set; }

    public ICollection<SelectListItem> Providers { get; set; }
}
