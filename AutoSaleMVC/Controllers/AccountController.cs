﻿using System.Security.Claims;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Domain.ViewModels.Account;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IImageService _imageService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IImageService imageService,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            RegisterViewModel registerViewModel = new()
            {
                ReturnUrl = returnUrl
            };
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl)
        {
            registerViewModel.ErrorMessages.Clear();
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                IResponse<Image>? response = null;
                if (registerViewModel.Image is not null)
                {
                    response = await _imageService.CreateAsync(registerViewModel.Image);

                    if (response.Code is not ResponseCode.Ok)
                    {
                        return View("Error");
                    }
                }
                
                User user = new()
                {
                    Name = registerViewModel.Name,
                    Surname = registerViewModel.Surname,
                    LastName = registerViewModel.LastName,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    ImageId = response?.Data.Id
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role.User.ToString());
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                
                foreach (var error in result.Errors)
                {
                    registerViewModel.ErrorMessages.Add(error.Description);
                }
               
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            LoginViewModel loginViewModel = new()
            {
                ReturnUrl = returnUrl ?? Url.Content("~/")
            };
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            loginViewModel.ErrorMessages.Clear();
            loginViewModel.ReturnUrl = returnUrl;
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                
                loginViewModel.ErrorMessages.Add("User not found. Incorrect password or email");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

                if (user is null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", 
                    new { userId = user.Id, code, email = forgotPasswordViewModel.Email },
                    protocol: HttpContext.Request.Scheme);

                var response = await _emailService.SendAsync(forgotPasswordViewModel.Email, 
                    "Reset Email Confirmation", 
                    $"Please reset email by going to this <a href=\"{callbackUrl}\">link</a>");

                if (response.Code is not ResponseCode.Ok)
                {
                    return View("Error");
                }

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(forgotPasswordViewModel);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string? code = null, string? email = null)
        {
            return code is null || email is null 
                ? View("Error") 
                : View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);

                if (user is null)
                {
                    ModelState.AddModelError("Email", "User not found. Incorrect email entered");
                    return View(resetPasswordViewModel);
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            
            return View(resetPasswordViewModel);
        }
        
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirect = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            returnUrl ??= Url.Content("~/");

            if (remoteError is not null)
            {
                ModelState.AddModelError(string.Empty, "Error from external provider");
                return View("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                return RedirectToAction("Login");
            }
            
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                ExternalLoginViewModel externalLoginViewModel = new()
                {
                    ReturnUrl = returnUrl,
                    ProviderDisplayName = info.ProviderDisplayName ?? "",
                    Email = email
                };
                return View("ExternalLoginConfirmation", externalLoginViewModel);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel externalLoginViewModel, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if (info is null)
                {
                    return View("Error");
                }

                var user = new User()
                {
                    UserName = externalLoginViewModel.Email,
                    Email = externalLoginViewModel.Email,
                    PhoneNumber = externalLoginViewModel.PhoneNumber,
                    Name = externalLoginViewModel.Name,
                    Surname = externalLoginViewModel.Surname,
                    LastName = externalLoginViewModel.LastName
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role.User.ToString());
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnUrl);
                    }
                }
                ModelState.AddModelError("Email", "User already exists");
            }

            externalLoginViewModel.ReturnUrl = returnUrl;
            return View(externalLoginViewModel);
        }
        
        [HttpGet]
        public IActionResult ExternalLoginConfirmation()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
        
        
        
        

    }
}