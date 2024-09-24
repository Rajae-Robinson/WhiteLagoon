using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(ModelState.IsValid) {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");            
            }
            return View();
        }

        public IActionResult Update(int Id) {
            Villa? villa = _db.Villas.FirstOrDefault(u => u.Id == Id);
            if (villa == null) {
                return NotFound();
            }
            return View(villa);
        }

    }
}