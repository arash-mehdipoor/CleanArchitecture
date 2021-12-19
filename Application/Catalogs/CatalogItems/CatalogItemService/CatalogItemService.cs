using Application.Dtos;
using Application.Interfaces.Context;
using AutoMapper;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Catalogs.CatalogItems.CatalogItemService
{
    public class CatalogItemService : ICatalogItemService
    {

        private readonly IDatabaseContext context;
        private readonly IMapper mapper;

        public CatalogItemService(IDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<CatalogBrandDto> GetBrand()
        {
            var brands = context.CatalogBrands
           .OrderBy(p => p.Brand).Take(500).ToList();

            var data = mapper.Map<List<CatalogBrandDto>>(brands);
            return data;
        }

        public List<ListCatalogTypeDto> GetCatalogType()
        {
            var types = context.CatalogTypes
               .Include(p => p.ParentCatalogType)
               .Include(p => p.ParentCatalogType)
               .ThenInclude(p => p.ParentCatalogType.ParentCatalogType)
                .Include(p => p.SubType)
                .Where(p => p.ParentCatalogTypeId != null)
                .Where(p => p.SubType.Count == 0)
                 .Select(p => new { p.Id, p.Type, p.ParentCatalogType, p.SubType })
                                .ToList()
                .Select(p => new ListCatalogTypeDto
                {
                    Id = p.Id,
                    Type = $"{p?.Type ?? ""} - {p?.ParentCatalogType?.Type ?? ""} - {p?.ParentCatalogType?.ParentCatalogType?.Type ?? ""}"
                }).ToList();
            return types;
        }

        public PaginatedItemsDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.CatalogItems
                .Include(p => p.CatalogType)
                .Include(p => p.CatalogBrand)
                .ToPaged(page, pageSize, out rowCount)
                .OrderByDescending(p => p.Id)
                .Select(p => new CatalogItemListItemDto
                {
                    Id = p.Id,
                    Brand = p.CatalogBrand.Brand,
                    Type = p.CatalogType.Type,
                    AvailableStock = p.AvailableStock,
                    MaxStockThreshold = p.MaxStockThreshold,
                    RestockThreshold = p.RestockThreshold,
                    Name = p.Name,
                    Price = p.Price,
                }).ToList();

            return new PaginatedItemsDto<CatalogItemListItemDto>(page, page, rowCount, data);

        }

        public class CatalogItemListItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public string Type { get; set; }
            public string Brand { get; set; }
            public int AvailableStock { get; set; }
            public int RestockThreshold { get; set; }
            public int MaxStockThreshold { get; set; }
        }

    }
}
