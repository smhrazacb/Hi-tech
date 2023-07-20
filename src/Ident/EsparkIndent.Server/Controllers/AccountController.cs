using EsparkIndent.Server.Entities;
using EsparkIndent.Server.Services;
using EsparkIndent.Server.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EsparkIndent.Server.Controllers;

/// <summary>
/// Controller responsible for Account Registration, Authentication and Authorization.
/// </summary>
[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly ApplicationDbContext _applicationDbContext;
    private static bool _databaseChecked;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _smsSender = smsSender;
        _applicationDbContext = applicationDbContext;
    }

    /// <summary>
    /// HTTP GET action for displaying the login view.
    /// </summary>
    /// <param name="returnUrl">The URL to redirect to after successful login.</param>
    /// <returns>The login view.</returns>
    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// HTTP POST action for login.
    /// </summary>
    /// <param name="model">model is passed to validate with database</param>
    /// <param name="returnUrl">The URL to redirect to after validation.</param>
    /// <returns>
    ///     If result Succeeded     -> return desired Url
    ///     If result Required 2FA  -> return sendcode and 
    ///     If result lockedout     -> return view of Lockedout
    ///     else                    -> Invalid login attempt
    /// </returns>
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        EnsureDatabaseCreated(_applicationDbContext);
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    /// <summary>
    /// HTTP GET action for Registration
    /// </summary>
    /// <param name="returnUrl"> The URl to redirect after successful registration</param>
    /// <returns>The Registration View</returns>
    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// HTTP POST action for registration
    /// </summary>
    /// <param name="model">Model credentials send for storing in DB</param>
    /// <param name="returnUrl">The Url after successful registration</param>
    /// <returns>
    ///     If modelstateValid && result succeeded -> Create user and logged in
    ///     else                                   -> AddErrors and display
    ///     
    /// </returns>
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        EnsureDatabaseCreated(_applicationDbContext);
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Context.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            AddErrors(result);
            
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    /// <summary>
    /// HTTP Post action for Logoff
    /// </summary>
    /// <returns>Return to HomePage after Singing Out</returns>
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    /// <summary>
    /// HTTP POST action for external Login
    /// </summary>
    /// <param name="provider">User credentials who is trying to login</param>
    /// <param name="returnUrl">Redirect to URL (uses External Login Callback method for Key)</param>
    /// <returns>Successfully Login the user</returns>
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        EnsureDatabaseCreated(_applicationDbContext);
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    /// <summary>
    /// HTTP GET action for ExternalLogin (used a helper function taking arguments from ExternalLogin method)
    /// </summary>
    /// <param name="returnUrl">redirects to the URL</param>
    /// <returns>
    ///     If info is Null         -> Return to Login Page
    ///     If result Succeeded     -> return desired Url (with sucessfully Login the user)
    ///     If result Required 2FA  -> return sendcode and 
    ///     If result lockedout     -> return view of Lockedout
    ///     else                    -> Redirect to URL, check the provider info in DB, return View
    /// Returns the Key/Code after successfull Login
    /// </returns>
    // GET: /Account/ExternalLoginCallback
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            return RedirectToAction(nameof(Login));
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            return RedirectToLocal(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
        }
        if (result.IsLockedOut)
        {
            return View("Lockout");
        }
        else
        {
            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
        }
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return View("ExternalLoginFailure");
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
            }
            AddErrors(result);
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    // GET: /Account/ConfirmEmail
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId is null || code is null)
        {
            return View("Error");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return View("Error");
        }
        var result = await _userManager.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    /// <summary>
    /// View Action for Forget Password
    /// </summary>
    /// <returns></returns>
    // GET: /Account/ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    /// <summary>
    /// HTTP POST action for Forgot Password
    /// </summary>
    /// <param name="model">validate the model with database</param>
    /// <returns>Return View Action for Recover Pass</returns>
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            // Send an email with this link
            //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Context.Request.Scheme);
            //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
            //   "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            //return View("ForgotPasswordConfirmation");
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    /// <summary>
    /// View Action for Cofirmation Forgot Password
    /// </summary>
    /// <returns></returns>
    // GET: /Account/ForgotPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    /// <summary>
    /// HTTP GET request for Reset Password
    /// </summary>
    /// <param name="code">code from model</param>
    /// <returns>
    ///     if code is nul -> return "ERROR"
    ///     if code is not null -> return View
    /// </returns>
    // GET: /Account/ResetPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
        return code is null ? View("Error") : View();
    }

    /// <summary>
    /// HTTP POST Action for Resetting Password
    /// </summary>
    /// <param name="model">model of the user</param>
    /// <returns>
    ///     if ModelState Valid     --> Return View
    ///     if user is null         --> Redirect to account Reset Pass Confirmation View
    ///     if result is suceeded   --> Redirect to account Reset Pass Confirmation View
    /// </returns>
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user is null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        }
        AddErrors(result);
        return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    //
    // GET: /Account/SendCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
        {
            return View("Error");
        }
        var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /Account/SendCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendCode(SendCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
        {
            return View("Error");
        }

        // Generate the token and send it
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
        if (string.IsNullOrWhiteSpace(code))
        {
            return View("Error");
        }

        var message = "Your security code is: " + code;
        if (model.SelectedProvider == "Email")
        {
            await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
        }
        else if (model.SelectedProvider == "Phone")
        {
            await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
        }

        return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
    }

    //
    // GET: /Account/VerifyCode
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
    {
        // Require that the user has already logged in via username/password or external login
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
        {
            return View("Error");
        }
        return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /Account/VerifyCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // The following code protects for brute force attacks against the two factor codes.
        // If a user enters incorrect codes for a specified amount of time then the user account
        // will be locked out for a specified amount of time.
        var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
        if (result.Succeeded)
        {
            return RedirectToLocal(model.ReturnUrl);
        }
        if (result.IsLockedOut)
        {
            return View("Lockout");
        }
        else
        {
            ModelState.AddModelError("", "Invalid code.");
            return View(model);
        }
    }

    #region Helpers

    // The following code creates the database and schema if they don't exist.
    // This is a temporary workaround since deploying database through EF migrations is
    // not yet supported in this release.
    // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
    // when publishing your application.
    private static void EnsureDatabaseCreated(ApplicationDbContext context)
    {
        if (!_databaseChecked)
        {
            _databaseChecked = true;
            context.Database.EnsureCreated();
        }
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    private async Task<ApplicationUser> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

    #endregion
}
