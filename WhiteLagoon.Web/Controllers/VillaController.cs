using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index");            
            }
            return View();
        }

        public IActionResult Update(int Id) {
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == Id);
            if (villa == null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if(ModelState.IsValid) {
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction("Index");            
            }
            return View();
        }

        public IActionResult Delete(int Id) {
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == Id);
            if (villa is null) {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (villa is not null) {
                _unitOfWork.Villa.Remove(villa);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index");            
            }
            TempData["error"] = "Failed to delete the villa.";
            return View();
        }


    }
}