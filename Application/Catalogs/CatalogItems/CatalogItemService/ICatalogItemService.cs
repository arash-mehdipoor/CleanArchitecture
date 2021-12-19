using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Application.Catalogs.CatalogItems.CatalogItemService.CatalogItemService;

namespace Application.Catalogs.CatalogItems.CatalogItemService
{
    public interface ICatalogItemService
    {
        List<CatalogBrandDto> GetBrand();
        List<ListCatalogTypeDto> GetCatalogType();
        PaginatedItemsDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize);
    }
}
