using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(v => v.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            
            if (roomNumberExists) {
                TempData["error"] = "Room number already exists.";
            }

            if(ModelState.IsValid && !roomNumberExists) {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index");            
            }
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {
            if(ModelState.IsValid) {
                _db.VillaNumbers.Update(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index");            
            }
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }

        public IActionResult Delete(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            VillaNumber? villaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (villaNumber is not null) {
                _db.VillaNumbers.Remove(villaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");            
            }
            TempData["error"] = "Failed to delete the villa number.";
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }
    }
}