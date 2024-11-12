using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pharmace.Helper;
using pharmace.Models;

namespace pharmace.Controllers
{
    public class OfferController : Controller
    {
        PharmacyContext context = new PharmacyContext();

        public IActionResult offerHome(int? page)
        {
            List<offerDTO> offerList = new List<offerDTO>();
            offerList =context.Offers.Include(c=>c.proudect)
                .Select(c=> new offerDTO
                {
                    idprodect=c.proudect.Id,
                    price=(float)c.proudect.Pricecycle,
                    rate=c.presentage,
                    newprice=c.proudect.Price,
                    image=c.proudect.image
                }).ToList();
            var pageSize = 20;
            var totalStudents = offerList.Count();

            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            page = Math.Max(page ?? 1, 1);
            page = Math.Min(page ?? 1, totalPages);

            ViewBag.CurrentPage = page;

            var students = offerList
                .Skip(((int)page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            return View(students);
        }
    }
}
