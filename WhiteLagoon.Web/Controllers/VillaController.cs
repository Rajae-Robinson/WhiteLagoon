using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if(obj.Image is not null) {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images/villa");

                    using var fileStream = new FileStream(Path.Combine(imagePath, filename), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"/images/Villa/" + filename;
                } else {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
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
                if(obj.Image is not null) {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images/villa");

                    if(!string.IsNullOrEmpty(obj.ImageUrl)) {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));

                        if (System.IO.File.Exists(oldImagePath)) 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, filename), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"/images/Villa/" + filename;
                }
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
            if (villa is not null) 
            {
                if(!string.IsNullOrEmpty(villa.ImageUrl)) {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath)) 
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

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