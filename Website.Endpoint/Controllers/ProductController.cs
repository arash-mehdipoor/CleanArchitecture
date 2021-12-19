﻿using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Microsoft.AspNetCore.Mvc;

namespace Website.Endpoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
        private readonly IGetCatalogItemPDPService getCatalogItemPDPService;
        public ProductController(IGetCatalogIItemPLPService
            getCatalogIItemPLPService
            , IGetCatalogItemPDPService getCatalogItemPDPService)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService; 
        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var data = getCatalogIItemPLPService.Execute(page, pageSize);
            return View(data);
        }

        public IActionResult Details(int Id)
        {
            var data = getCatalogItemPDPService.Execute(Id);
            return View(data);
        }

    }
}
