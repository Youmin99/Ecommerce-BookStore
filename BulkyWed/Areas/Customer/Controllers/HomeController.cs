using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.Repository.IRepository;
using Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Utility;

namespace BulkyWed.Areas.Customer.Controllers;

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

      public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

        return View(productList);
    }

    public IActionResult Detail(int Id)
    {
        ShoppingCart cartObj = new()
        {
            Count = 1,
            ProductId = Id,
            Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == Id, includeProperties: "Category,CoverType"),
        };

		return View(cartObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Detail(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity; 
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.ApplicationUserId = claim.Value;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
            u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);


        if (cartFromDb == null)
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
        }
        else
        {
            _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
        }

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
