using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public string ISBN { get; set; }

		[Required]
		public string Author { get; set; }

		[Required]
		[Range(1, 10000)]
		[Display(Name = "List Price")]
		public double ListPrice { get; set; }

		[ValidateNever]
		[Range(1, 100)]
		[Display(Name = "DiscountPercent")]
		public double DiscountPercent { get; set; }

		[ValidateNever]
		[Range(1, 10000)]
		[Display(Name = "DiscountAmount")]
		public double DiscountAmount { get; set; }

		[Required]
		[Display(Name = "Price")]
		[Range(1, 10000)]
		public double Price { get; set; }

		[ValidateNever]
		public string ImageUrl { get; set; }

		[Required]
		[Display(Name = "Category")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }

		[Required]
		[Display(Name = "Cover Type")]
		public int CoverTypeId { get; set; }
		[ValidateNever]
		public CoverType CoverType { get; set; }
	}
}