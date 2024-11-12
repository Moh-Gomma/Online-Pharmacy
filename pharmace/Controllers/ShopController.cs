using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pharmace.Helper;
using pharmace.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace pharmace.Controllers
{
    public class ShopController : Controller
    {
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }
        PharmacyContext context = new PharmacyContext();
        public IActionResult ShopHome(int? page, string? listGroupRadio,int? rang,string? catstatus ,int? cat)
        {
            var Proudect = context.Proudects.ToList();
            ViewBag.catt=context.Categories.ToList();

            var randomOffer = context.Offers
                     .Include(o => o.proudect)
                     .OrderBy(o => Guid.NewGuid())
                     .FirstOrDefault();

            ViewBag.RandomOffer = randomOffer;


            ViewBag.rang=rang;  
            ViewBag.catstatus=catstatus;
            ViewBag.cat=cat;
            ViewBag.listGroupRadio=listGroupRadio;
            if (!string.IsNullOrEmpty(listGroupRadio))
            {
                Proudect = Proudect.Where(c => c.category.Name== listGroupRadio).ToList();
            }   
            if (cat != null)
            {
                Proudect = Proudect.Where(c => c.category.Id== cat).ToList();
            }
            if (rang != null && rang !=0)
            {
                Proudect = Proudect.Where(c => c.Price <= rang).ToList();
            }
            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();

                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();

                }
            }

            ViewBag.status = catstatus;
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }

            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.students = students;
            return View(students);
        }
        public IActionResult ProductDetail(int id)
        {
            List<int> offersid = context.Offers.Select(c => c.proudectId).ToList();
            if (offersid.Contains(id))
            {
                ViewBag.Proudect = context.Proudects.OrderBy(p => Guid.NewGuid()).Take(10).ToList();
                var x = context.Proudects.Where(c => c.Id == id).FirstOrDefault();
                ViewBag.offerList = context.Offers.Include(c => c.proudect)
                    .Where(c=>c.proudect.Id==id)
                    .Select(c => new offerDTO
                    {
                        idprodect = c.proudect.Id,
                        price = (float)c.proudect.Pricecycle,
                        rate = c.presentage,
                        newprice = c.proudect.Price,
                        image = c.proudect.image
                    }).FirstOrDefault();
                if (x == null)
                {
                    return RedirectToAction("ShopHome");
                }
                return View("ProductDetailoffer", x);

            }
            else
            {

                ViewBag.Proudect = context.Proudects.OrderBy(p => Guid.NewGuid()).Take(10).ToList();
                var x = context.Proudects.Where(c => c.Id == id).FirstOrDefault();
                if (x == null)
                {
                    return RedirectToAction("ShopHome");
                }
                return View(x);

            }

        }


        public IActionResult showone(int? page, string? catstatus)
        {
            string category = "الدواء";
            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();
            if (category != null)
            {
                Proudect = Proudect.Where(c => c.category.Name == category).ToList();
            }
            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();

                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();

                }
            }
            ViewBag.status = catstatus;
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }
            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }  
        public IActionResult showtwo(int? page, string? catstatus)
        {
            string category = "العناية بالشعر";

            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();
            if (category != null)
            {
                Proudect = Proudect.Where(c => c.category.Name == category).ToList();
            }
            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();

                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();

                }
            }
            ViewBag.status = catstatus;
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }
            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }  
        public IActionResult showthree(int? page, string? catstatus)
        {
            string category = "العناية بالبشرة";

            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();
            if (category != null)
            {
                Proudect = Proudect.Where(c => c.category.Name == category).ToList();
            }
            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();

                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();

                }
            }
            ViewBag.status = catstatus;
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }
            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }
        public IActionResult showfour(int? page, string? catstatus)
        {
            // Define the categories we want to show
            List<string> categories = new List<string> { "المكملات الغذائية", "الفيتامينات" };

            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();

            // Filter products by the specified categories
            Proudect = Proudect.Where(c => categories.Contains(c.category.Name)).ToList();

            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();
                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();
                }
            }

            ViewBag.status = catstatus;

            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }

            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }
        public IActionResult showfive(int? page, string? catstatus)
        {
            string category = "الصحة الجنسية";

            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();
            if (category != null)
            {
                Proudect = Proudect.Where(c => c.category.Name == category).ToList();
            }
            if (!string.IsNullOrEmpty(catstatus))
            {
                if (catstatus == "lowest")
                {
                    Proudect = Proudect.OrderBy(c => c.Price).ToList();

                }
                else
                {
                    Proudect = Proudect.OrderByDescending(c => c.Price).ToList();

                }
            }
            ViewBag.status = catstatus;
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }
            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }

        [HttpGet]
        public IActionResult search(int? page, string? query)
        {
            var Proudect = context.Proudects.ToList();
            ViewBag.cat = context.Categories.ToList();
            if (!string.IsNullOrEmpty(query))
            {

                    Proudect = Proudect.Where(c=>c.Name.ToLower().StartsWith(query.ToLower())).ToList();

                
            }
            if (Proudect.Count == 0)
            {
                List<Proudect> list = new List<Proudect>();
                return View(list);
            }
            var pageSize = 20;
            var totalStudents = Proudect.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = Proudect
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }   



        public IActionResult Filtered()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addmulti(int id, int? count)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Create", "Home");
            }

            var userid = GetLoggedInUserId();

            var existingCartItem = context.Carts
                .FirstOrDefault(c => c.userId == userid && c.proudectId == id);

            if (existingCartItem != null)
            {
                existingCartItem.count += count ?? 1;
            }
            else
            {
                var cart = new Cart
                {
                    userId = userid,
                    proudectId = id,
                    count = count ?? 1
                };
                context.Carts.Add(cart);
            }

            context.SaveChanges();

            return RedirectToAction("ProductDetail", "Shop", new { id = id });
        }
    }
}
