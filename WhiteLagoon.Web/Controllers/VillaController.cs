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
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index");            
            }
            return View();
        }

        public IActionResult Update(int Id) {
            Villa? villa = _db.Villas.FirstOrDefault(u => u.Id == Id);
            if (villa == null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if(ModelState.IsValid) {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction("Index");            
            }
            return View();
        }

        public IActionResult Delete(int Id) {
            Villa? villa = _db.Villas.FirstOrDefault(u => u.Id == Id);
            if (villa is null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villa = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);
            if (villa is not null) {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index");            
            }
            TempData["error"] = "Failed to delete the villa.";
            return View();
        }


    }
}