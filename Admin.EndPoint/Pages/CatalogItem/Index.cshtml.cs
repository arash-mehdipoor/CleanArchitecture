using Application.Catalogs.CatalogItems.CatalogItemService;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Application.Catalogs.CatalogItems.CatalogItemService.CatalogItemService;

namespace Admin.EndPoint.Pages.CatalogItem
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogItemService catalogItemService;

        public IndexModel(ICatalogItemService catalogItemService)
        {
            this.catalogItemService = catalogItemService;
        }
        public PaginatedItemsDto<CatalogItemListItemDto> CatalogItems { get; set; }
        public void OnGet(int page = 1, int pageSize = 100)
        {
            CatalogItems = catalogItemService.GetCatalogList(page, pageSize);
        }
    }
}
