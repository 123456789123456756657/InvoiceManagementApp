using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InvoiceManagementApp.Models;
using InvoiceManagementApp.Service;
using InvoiceManagementApp.Service.Users;
using InvoiceManagementApp.Models.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using InvoiceManagementApp.Repository.IRepository;
using System;

namespace InvoiceManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly GoogleReCaptchaSettings _reCaptchaSettings;
        private readonly IEmailService _emailService;

        public AccountController(UserService userService, IOptions<GoogleReCaptchaSettings> reCaptchaSettings, IEmailService emailService)
        {
            _userService = userService;
            _reCaptchaSettings = reCaptchaSettings.Value;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, [FromForm(Name = "g-recaptcha-response")] string gRecaptchaResponse)
        {
            if (!await IsRecaptchaValid(gRecaptchaResponse))
            {
                ViewBag.ErrorMessage = "Invalid reCAPTCHA. Please try again.";
                return View();
            }
            var user = await _userService.AuthenticateAsync(username, password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
            if (!user.IsEmailVerified)
            {
                ViewBag.ErrorMessage = "Please verify your email before logging in.";
                return View();
            }
            HttpContext.Session.SetString("Username", user.Username);


            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            user.VerificationToken = Guid.NewGuid().ToString();
            user.IsEmailVerified = false;

            await _userService.RegisterAsync(user);

            string verificationLink = Url.Action("VerifyEmail","Account", new { token = user.VerificationToken },Request.Scheme);

            await _emailService.SendEmailAsync(user.Email,"Email Verification",$"Please verify your email by clicking <a href='{verificationLink}'>here</a>.");

            ViewBag.SuccessMessage = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to the Login page
            return RedirectToAction("Login", "Account");
        }
        private async Task<bool> IsRecaptchaValid(string gRecaptchaResponse)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={_reCaptchaSettings.SecretKey}&response={gRecaptchaResponse}",
                null
            );

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);
            return result.success == "true";
        }

       
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var user = await _userService.GetByVerificationTokenAsync(token);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid or expired token.";
                return RedirectToAction("Login");
            }

            user.IsEmailVerified = true;
            user.VerificationToken = null; // Clear the token after verification
            await _userService.UpdateAsync(user);
            
            ViewBag.SuccessMessage = "Email verified successfully. You can now log in.";
            return RedirectToAction("Login");
        }

    }
}
