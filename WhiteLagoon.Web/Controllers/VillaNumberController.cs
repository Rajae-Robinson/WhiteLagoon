using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize]
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            
            if (roomNumberExists) {
                TempData["error"] = "Room number already exists.";
            }

            if(ModelState.IsValid && !roomNumberExists) {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));            
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
                _unitOfWork.VillaNumber.Update(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction(nameof(Index));            
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }

        public IActionResult Delete(int villaNumberId) {
            VillaNumberVM villaNumberVM = new() {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber is null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            VillaNumber? villaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (villaNumber is not null) {
                _unitOfWork.VillaNumber.Remove(villaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));            
            }
            TempData["error"] = "Failed to delete the villa number.";
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(obj);
        }
    }
}