using Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.Repository.IRepository;
using Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Utility;
using DataAccess.Repository;

namespace BulkyWed.Areas.Customer.Controllers;

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ProductDetailVM ProductDetailVM { get; set; }

      public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index(string search="", string fillter="")
    {
        IEnumerable<Product> productList;

		if (search != "" && search != null)
		{
			productList = _unitOfWork.Product.GetAll(includeProperties: @fillter).Where(p=>p.Title.Contains(search));
		}
		else
		{
			productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
		}


		return View(productList);
    }

    public IActionResult Detail(int productId)
    {
        ProductDetailVM = new ProductDetailVM()
        {
            Cart = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType")
            },
            Comments = _unitOfWork.Comment.GetAll(u => u.ProductId == productId, includeProperties: "ApplicationUser"),
            Comment = new()
         };
      

		return View(ProductDetailVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Detail(ProductDetailVM productDetail)
    {
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if(productDetail.Comment != null)
        {
            productDetail.Comment.ApplicationUserId = claim.Value;
            productDetail.Comment.ProductId = productDetail.Cart.ProductId;
            _unitOfWork.Comment.Add(productDetail.Comment);

            _unitOfWork.Save();

            return RedirectToAction("Detail", new { productId = productDetail.Cart.ProductId });
        }

        productDetail.Cart.ApplicationUserId = claim.Value;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
            u => u.ApplicationUserId == claim.Value && u.ProductId == productDetail.Cart.ProductId);

        if (cartFromDb == null)
		{
			_unitOfWork.ShoppingCart.Add(productDetail.Cart);
		}
		else
		{
			_unitOfWork.ShoppingCart.IncrementCount(cartFromDb, productDetail.Cart.Count);
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


    [Authorize]
    public IActionResult delete(int commentId)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
        var comment = _unitOfWork.Comment.GetFirstOrDefault(u => u.Id == commentId);
       
        if(claim.Value == comment.ApplicationUserId)
        {
            _unitOfWork.Comment.Remove(comment);

            _unitOfWork.Save();

            return RedirectToAction("Detail", new { productId = comment.ProductId });
        }
        else
        {
            return RedirectToAction("Detail", new { productId = comment.ProductId });
        }

    }

	[Authorize]
	public IActionResult Plus(int productId)
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

		var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == productId);

        if(cart == null)
        {
            ShoppingCart Cart = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType")
            };
			Cart.ApplicationUserId = claim.Value;
			_unitOfWork.ShoppingCart.Add(Cart);
		}
        else
        {
			_unitOfWork.ShoppingCart.IncrementCount(cart, 1);
		}
		_unitOfWork.Save();
		return RedirectToAction(nameof(Index));
	}

	[Authorize]
	public IActionResult Minus(int productId)
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

		var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == productId);
		if (cart.Count <= 1)
		{
			_unitOfWork.ShoppingCart.Remove(cart);
		}
		else
		{
			_unitOfWork.ShoppingCart.DecrementCount(cart, 1);
		}
		_unitOfWork.Save();
		return RedirectToAction(nameof(Index));
	}



}


