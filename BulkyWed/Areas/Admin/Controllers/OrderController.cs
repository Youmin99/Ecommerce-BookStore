using DataAccess.Repository.IRepository;
using Models;
using Models.ViewModels;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
		public class OrderController : Controller
		{
			private readonly IUnitOfWork _unitOfWork;
            [BindProperty]
            public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public IActionResult Index()
			{
				return View();
			}

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
            };
            return View(OrderVM);
        }

        #region API CALLS
        [HttpGet]
			public IActionResult GetAll()
			{
				IEnumerable<OrderHeader> orderHeaders;
				orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
				return Json(new { data = orderHeaders });
			}
			#endregion
		}

}
