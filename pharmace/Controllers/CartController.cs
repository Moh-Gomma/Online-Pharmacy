using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmace.Models;

namespace pharmace.Controllers
{
    public class CartController : Controller
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
        public IActionResult CartPage()
        {
            ViewBag.offersid = context.Offers.Select(c => c.proudectId).ToList();

            var cart = context.Carts
                .Include(c => c.proudect)
                .Where(c => c.userId == GetLoggedInUserId()).ToList();
            float sum = 0;
            foreach (var cartItem in cart)
            {
                sum += (cartItem.proudect.Price * cartItem.count);
            }
            //ViewBag.Sum = sum + 10;
            ViewBag.Sum = sum ;
            return View(cart);
        }
        public IActionResult Succes()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            ViewBag.offersid = context.Offers.Select(c => c.proudectId).ToList();

            var cart = context.Carts
                .Include(c => c.proudect)
                .Where(c => c.userId == GetLoggedInUserId()).ToList();
            float sum = 0;
            foreach (var cartItem in cart)
            {
                sum += (cartItem.proudect.Price * cartItem.count);
            }
            //ViewBag.Sum = sum + 10;
            ViewBag.Sum = sum ;
            return View(cart);
        }

        [HttpPost]
        public IActionResult orderr(string fname, string sname, string Email, string Phone, string sPhone, string address)
        {
            var cart = context.Carts
                .Include(c => c.proudect)
                .Where(c => c.userId == GetLoggedInUserId()).ToList();
            float sum = 0;
            foreach (var cartItem in cart)
            {
                sum += (cartItem.proudect.Price * cartItem.count);
            }
            //ViewBag.Sum = sum + 10;
            ViewBag.Sum = sum ;

            var x = new Orders()
            {
                fname = fname,
                sname = sname,
                address = address,
                email = Email,
                phone = Phone,
                sphone = sPhone,
                Totalprice = sum,
                states = false,
                userId = GetLoggedInUserId(),
                Date = DateTime.Now,

            };
            context.Orders.Add(x);
            context.SaveChanges();
            foreach (var cartItem in cart)
            {
                var c = new orderprodect()
                {
                    proudectId = cartItem.proudectId,
                    orderId = x.Id,
                    count = cartItem.count,
                };
                context.orderprodects.Add(c);
                context.SaveChanges();
                cartItem.proudect.count--;
                context.SaveChanges();
            };

            foreach (var cartItem in cart)
            {
                context.Carts.Remove(cartItem);
                context.SaveChanges();
            }

            return Json(new { success = true, message = "New order placed!", redirectUrl = Url.Action("CartPage", "Cart") });

        }

        public IActionResult CheckNewOrders()
        {
            var hasNewOrder = context.Orders
                .Any(o => o.Date >= DateTime.Now.AddSeconds(-30));

            return Json(new { hasNewOrder });
        }




        [HttpPost]
        public IActionResult Delete(int id)
        {
            var c = context.Carts
                .Where(c => c.userId == GetLoggedInUserId() && c.proudectId == id)
                .FirstOrDefault();
            context.Carts.Remove(c);
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult min(int id)
        {
            var c = context.Carts
                .Where(c => c.userId == GetLoggedInUserId() && c.proudectId == id)
                .FirstOrDefault();
            if (c.count == 1)
            {
                return Json(new { success = false });
            }
            c.count = c.count - 1;
            context.SaveChanges();
            return Json(new { success = true });
        }



        [HttpPost]
        public IActionResult max(int id)
        {
            var c = context.Carts
                .Where(c => c.userId == GetLoggedInUserId() && c.proudectId == id)
                .FirstOrDefault();
            c.count = c.count + 1;
            context.SaveChanges();
            return Json(new { success = true });
        }


        public int GetCartCount()
        {
            var count = context.Carts.Count(c => c.userId == GetLoggedInUserId());
            return count;
        }

        [HttpGet]
        public JsonResult UpdateCartBadge()
        {
            var cartCount = GetCartCount();
            return Json(new { count = cartCount });
        }

    }
}
