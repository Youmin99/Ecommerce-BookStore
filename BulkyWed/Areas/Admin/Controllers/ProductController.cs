using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataAccess.Data;
using Utility;
using Models;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;

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
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();

	      	return View(objProductList);
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
	public IActionResult Upsert(ProductVM obj, IFormFile file)
	{

		if (ModelState.IsValid)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			if(file != null)
			{
				string fileName =Guid.NewGuid().ToString() +Path.GetExtension(file.FileName);
				string productPath =Path.Combine(wwwRootPath, @"images\product");

				using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
				obj.Product.ImageUrl = @"images\product\" + fileName;
			}
			_unitOfWork.Product.Add(obj.Product);
			_unitOfWork.Save();
			TempData["success"] = "CoverType updated successfully";
			return RedirectToAction("Index");
		}
		else
		{
			obj.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			});
			obj.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			});

			return View(obj);
		}
		
	}
	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
		{
			return NotFound();
		}
		var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
		if (CoverTypeFromDbFirst == null)
		{
			return NotFound();
		}
		return View(CoverTypeFromDbFirst);
	}
	//POST
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public IActionResult DeletePOST(int? id)
	{
		var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
		if (obj == null)
		{
			return NotFound();
		}
		_unitOfWork.CoverType.Remove(obj);
		_unitOfWork.Save();
		TempData["success"] = "CoverType deleted successfully";
		return RedirectToAction("Index");

	}

}
