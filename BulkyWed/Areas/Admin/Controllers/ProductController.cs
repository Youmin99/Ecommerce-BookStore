using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataAccess.Data;
using Utility;
using Models;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace BulkyWed.Areas.Admin.Controllers;

    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
	    private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
		    _webHostEnvironment = webHostEnvironment;
		}

        public IActionResult Index()
        {
	      	return View();
        }

	//GET
	public IActionResult Upsert(int? id)
	{
		ProductVM productVM = new()
		{
			Product = new(),
			CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			}),
			CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			}),
		};

		if (id == null || id == 0)
		{
			return View(productVM);
		}
		else
		{
			productVM.Product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
			return View(productVM);

		}
	}
	//POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Upsert(ProductVM obj, IFormFile? file)
	{
	 if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(wwwRootPath, @"images\product");
					var extension = Path.GetExtension(file.FileName);

					if (obj.Product.ImageUrl != null)
					{
						var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}
					obj.Product.ImageUrl = @"\images\product\" + fileName + extension;

				}

				if (obj.Product.Price <= obj.Product.ListPrice)
				{
					obj.Product.DiscountPercent = Math.Ceiling(((obj.Product.ListPrice - obj.Product.Price) / obj.Product.ListPrice) * 100);
					obj.Product.DiscountAmount = ( obj.Product.ListPrice - obj.Product.Price);
				}
				else
				{
					throw new Exception("Price must be less than ListPrice");
				}

				if (obj.Product.Id == 0)
				{
					_unitOfWork.Product.Add(obj.Product);
				}
				else
				{
					_unitOfWork.Product.Update(obj.Product);
				}
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		
	}
	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
		{
			return NotFound();
		}
		var ProductFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
		if (ProductFromDbFirst == null)
		{
			return NotFound();
		}
		return View(ProductFromDbFirst);
	}
	//POST
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public IActionResult DeletePOST(int? id)
	{
		var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
		if (obj == null)
		{
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
		_unitOfWork.Save();
		TempData["success"] = "CoverType deleted successfully";
        return Json(new { success = true, message = "Delete Successful" });
    }



	[HttpGet]
	public IActionResult GetAll()
	{
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
	}

}
