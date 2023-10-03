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
		
		}


		return View(productVM);
	}
	//POST
	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Upsert(CoverType obj)
	{

		if (ModelState.IsValid)
		{
			_unitOfWork.CoverType.Update(obj);
			_unitOfWork.Save();
			TempData["success"] = "CoverType updated successfully";
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
