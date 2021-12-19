using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Microsoft.AspNetCore.Mvc;

namespace Website.Endpoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
 
        public ProductController(IGetCatalogIItemPLPService
            getCatalogIItemPLPService)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService; 
        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var data = getCatalogIItemPLPService.Execute(page, pageSize);
            return View(data);
        }

       
    }
}
