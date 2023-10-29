using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Comment
	{

        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [MaxLength(30)]
        [ValidateNever]
        public string review { get; set; }

		public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }



    }
}