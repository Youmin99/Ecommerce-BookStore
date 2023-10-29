using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ProductDetailVM
    {
        public ShoppingCart Cart { get; set; }
 
        public IEnumerable<Comment> Comments { get; set; }
        public Comment Comment { get; set; }

    }
}
