using HUc.Filters;
using HUc.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using pharmace.Models;
using System.Collections.Generic;
using pharmace.Helper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace pharmace.Controllers
{
    public class HomeController : Controller
    {
        private readonly PharmacyContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, PharmacyContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

       
        public IActionResult Index()
        {
            var carouselImages = _context.CarouselImages.Where(c => c.IsActive).ToList();

            if (carouselImages == null)
            {
                carouselImages = new List<CarouselImage>();
            }
            if (User.Identity.IsAuthenticated)
            {

                var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                if (role == "admin")
                {

                    return RedirectToAction("dashHome", "Dashboard");
                }
            }
            ViewBag.categories = _context.Categories.ToList();
            ViewBag.Proudect = _context.Proudects
                                  .OrderBy(p => Guid.NewGuid())
                                  .Take(10).ToList();
            ViewBag.prodectcat = _context.Proudects.Include(c => c.category)
                .Where(c => c.category.Name == "منتجات للنظافة الشخصية")
                .OrderBy(p => Guid.NewGuid())
                .Take(10).ToList();
            ViewBag.prodectvet = _context.Proudects.Include(c => c.category)
                .Where(c => c.category.Name == "الفيتامينات" || c.category.Name == "المكملات الغذائية")
                .OrderBy(p => Guid.NewGuid())
                .Take(10).ToList();
            var orders = _context.Offers
                .Include(c => c.proudect)
                .OrderBy(p => Guid.NewGuid())
                .Take(10)
                .ToList();

            if (orders.Count == 0)
            {
                List<Orders> list = new List<Orders>();
                ViewBag.offers = list;
                return View(carouselImages);
            }
            else
            {
                ViewBag.offers = orders;
                return View(carouselImages);
            }
        }
        public IActionResult contantUs()
        {
            return View();
        }

        public async Task<IActionResult> accountinfo()
        {
            List<Orders> orders = await _context.Orders.Include(c => c.user)
                .Where(c => c.user.Id == GetLoggedInUserId())
                .OrderByDescending(c => c.Id).ToListAsync();
            List<List<orderDTO>> ordersDTO = new List<List<orderDTO>>();
            foreach (var order in orders)
            {
                var p = _context.orderprodects.Include(c => c.proudect)
                    .Where(c => c.orderId == order.Id)
                    .Select(c => new orderDTO
                    {
                        name = c.proudect.Name,
                        count = c.count,
                        price = c.proudect.Price,
                        image = c.proudect.image
                    })
                    .ToList();
                ordersDTO.Add(p);
            }
            ViewBag.ordersDTO = ordersDTO;
            if (orders.Count == 0)
            {
                List<Orders> list = new List<Orders>();
                ViewBag.myorder = list;

                userpermations xx = _context.userpermations.Where(c => c.Id == GetLoggedInUserId()).FirstOrDefault();
                return View(xx);
            }

            ViewBag.myorder = orders;

            userpermations x = _context.userpermations.Where(c => c.Id == GetLoggedInUserId()).FirstOrDefault();
            return View(x);
        }



        [PreventLoggedInAccess]
        public ActionResult Create()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [PreventLoggedInAccess]
        public IActionResult signup()
        {
            return View();
        }

        [AuthorizeUser]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(string fname, string sname, string Email, string password, string passwordconfirm, string? role)
        {
            if (password != passwordconfirm)
            {
                ViewBag.Password = "كلمت السر غير متشابه";
                return View("signup");
            }

            if (_context.userpermations.Any(u => u.Email == Email))
            {
                ViewBag.Email = "البريد الإلكتروني مسجل بالفعل";
                return View("signup");
            }

            var verificationToken = Guid.NewGuid().ToString();
            var user = new userpermations
            {
                Email = Email,
                Password = CommonMethods.ConvertToEncrypt(password),
                fname = fname,
                lname = sname,
                Username = fname + " " + sname,
                Role = role ?? "user",
                VerificationToken = verificationToken,
                VerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
                IsVerified = false
            };

            _context.userpermations.Add(user);
            await _context.SaveChangesAsync();

            var verificationLink = Url.Action("VerifyEmail", "Home", new { token = verificationToken }, Request.Scheme);

            // Create a nice-looking email message
            var emailBody = $@"
                <h1>مرحبًا بك في صيدلية الباز!</h1>
                <p>
                    نحن سعداء بانضمامك إلى صيدلية الباز. 
                    لإنهاء عملية التسجيل، يرجى النقر على الرابط أدناه لتأكيد بريدك الإلكتروني:
                </p>
                <a href='{verificationLink}'>تأكيد البريد الإلكتروني</a>
                <p>
                    نشكرك على اختيارك صيدلية الباز، ونعدك بتوفير أفضل خدمة وأفضل المنتجات.
                </p>
                <p>
                    مع أطيب التمنيات،
                    <br>
                    فريق صيدلية الباز
                </p>
            ";

            await _emailService.SendEmailAsync(Email, "تأكيد البريد الإلكتروني", emailBody);

            return RedirectToAction("VerificationSent");
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.userpermations.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Json(new { success = false, message = "البريد الإلكتروني غير صحيح او غير مسجل " });
            }
            if (!CommonMethods.ConvertToDecrypt(user.Password).Equals(password))
            {
                return Json(new { success = false, message = " كلمة السر غير صحيحة." });
            }

            try
            {
                List<Claim> claims = new List<Claim>{
            new Claim("userId",  user.Id.ToString()),
            new Claim("role", user?.Role)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى." });
            }
        }

        [PreventLoggedInAccess]
        public IActionResult VerificationSent()
        {
            return View();
        }

        public async Task<IActionResult> VerifyEmail(string token)
        {
            var user = await _context.userpermations.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null || user.VerificationTokenExpiry < DateTime.UtcNow)
            {
                return View("VerificationFailed");
            }

            user.IsVerified = true;
            user.VerificationToken = null;
            user.VerificationTokenExpiry = null;
            await _context.SaveChangesAsync();

            return View("VerificationSuccess");
        }

        [PreventLoggedInAccess]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [PreventLoggedInAccess]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.userpermations.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "البريد الإلكتروني غير مسجل";
                return View();
            }

            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _context.SaveChangesAsync();

            var resetLink = Url.Action("ResetPassword", "Home", new { token = resetToken }, Request.Scheme);

            // Create a nice-looking email message
            var emailBody = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9;'>
                        <h1 style='color: #333;'>اعادة تعيين كلمة السر!</h1>

                        <p style='font-size: 16px; color: #555;'>
                            لإعادة تعيين كلمة المرور، يرجى النقر على الرابط أدناه:
                        </p>
                        <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px;'>إعادة تعيين كلمة المرور</a>

                        <p style='font-size: 16px; color: #555;'>
                            <strong>فريق صيدلية الباز</strong>
                        </p>
                    </div>
                ";

            await _emailService.SendEmailAsync(email, "إعادة تعيين كلمة المرور", emailBody);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [PreventLoggedInAccess]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [PreventLoggedInAccess]
        public IActionResult ResetPassword(string token)
        {
            var user = _context.userpermations.FirstOrDefault(u => u.ResetPasswordToken == token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                return View("VerificationFailed");
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.userpermations.FirstOrDefaultAsync(u => u.ResetPasswordToken == model.Token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "رابط إعادة تعيين كلمة المرور غير صالح أو منتهي الصلاحية");
                return View(model);
            }

            user.Password = CommonMethods.ConvertToEncrypt(model.Password);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            await _context.SaveChangesAsync();

            return RedirectToAction("ResetPasswordConfirmation");
        }
        [PreventLoggedInAccess]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string? firstName, string? lastName, string? email, string? phone, int id, string? address)
        {
            userpermations x = _context.userpermations.Where(c => c.Id == GetLoggedInUserId()).FirstOrDefault();
            x.Email = email ?? x.Email;
            x.Address = address ?? x.Address;
            x.PhoneNumber = phone ?? x.PhoneNumber;
            x.fname = firstName ?? x.fname;
            x.lname = lastName ?? x.lname;
            x.Username = firstName ?? x.fname + " " + lastName ?? x.lname;

            _context.SaveChanges();
            TempData["Message"] = "تم تحديث المعلومات بنجاح!";

            return RedirectToAction(nameof(accountinfo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, string currentPassword, string newPassword, string confirmPassword)
        {
            userpermations x = _context.userpermations.Where(c => c.Id == GetLoggedInUserId()).FirstOrDefault();
            if (newPassword != confirmPassword)
            {
                TempData["AlertMessage"] = "كلمة السر غير متطابقة";
                TempData["AlertType"] = "danger";
            }
            else if (CommonMethods.ConvertToEncrypt(currentPassword) == x.Password)
            {
                x.Password = CommonMethods.ConvertToEncrypt(newPassword);
                _context.SaveChanges();
                TempData["AlertMessage"] = "تم تحديث كلمة المرور بنجاح";
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertMessage"] = "كلمة السر الحالية غير صحيحة";
                TempData["AlertType"] = "danger";
            }
            return RedirectToAction("accountinfo");
        }

        [HttpPost]
        public IActionResult add(int id)
        {
            var userid = GetLoggedInUserId();

            var existingCartItem = _context.Carts.FirstOrDefault(c => c.userId == userid && c.proudectId == id);

            if (existingCartItem != null)
            {
                existingCartItem.count++;
                _context.SaveChanges();

                return Json(new { success = true, message = "تمت الاضافة" });
            }

            var newCartItem = new Cart
            {
                userId = userid,
                proudectId = id,
                count = 1
            };
            _context.Carts.Add(newCartItem);
            _context.SaveChanges();

            return Json(new { success = true, message = "تمت اضافة المنتج" });
        }




        public IActionResult About()
        {
            return View();
        }

    }
}