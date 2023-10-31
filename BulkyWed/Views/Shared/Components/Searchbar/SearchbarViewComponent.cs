using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataAccess.Data;
using Utility;
using Models;
using DataAccess.Repository.IRepository;
using BulkyWed.Views.Shared.Components.SPager;

namespace BulkyWed.Views.Shared.Components.Searchbar;


public class SearchbarViewComponent : ViewComponent
{
    public SearchbarViewComponent()
    {
    }

    public IViewComponentResult Invoke(/*SPager SearchPage*/)
    {
        return View("Default"/*,SearchPage*/);
    }



}
