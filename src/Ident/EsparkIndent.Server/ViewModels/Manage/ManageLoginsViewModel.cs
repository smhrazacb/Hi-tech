using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace EsparkIndent.Server.ViewModels.Manage;

public class ManageLoginsViewModel
{
    public IList<UserLoginInfo> CurrentLogins { get; set; }

    public IList<AuthenticationScheme> OtherLogins { get; set; }
}
