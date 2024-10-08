﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        [Authorize]
        public async Task Logout()
        {
            //clear the local cookies
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirects to the IDP linked to scheme
            // "OpenIdConnectDefaults.AuthenticationScheme"
            // so it can clear its own session/cookie
            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
