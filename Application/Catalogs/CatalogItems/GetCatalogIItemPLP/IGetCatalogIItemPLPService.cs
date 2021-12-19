﻿using Application.Catalogs.CatalogItems.UriComposer;
using Application.Dtos;
using Application.Interfaces.Context;
using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.GetCatalogIItemPLP
{
    public interface IGetCatalogIItemPLPService
    {
        PaginatedItemsDto<CatalogPLPDto> Execute(int page, int pageSize);
    }


    public class GetCatalogIItemPLPService : IGetCatalogIItemPLPService
    {
        private readonly IDatabaseContext context;
        private readonly IUriComposerService uriComposerService;

        public GetCatalogIItemPLPService(IDatabaseContext context, IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
        }

        public PaginatedItemsDto<CatalogPLPDto> Execute(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.CatalogItems
                .Include(p => p.CatalogItemImages)
                .OrderByDescending(p => p.Id)
                .PagedResult(page, pageSize, out rowCount)
                .Select(p => new CatalogPLPDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rate = 4,
                    Image = uriComposerService.ComposeImageUri(p.CatalogItemImages.FirstOrDefault().Src),
                }).ToList();
            return new PaginatedItemsDto<CatalogPLPDto>(page, pageSize, rowCount, data);
        }
    }

    public class CatalogPLPDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public byte Rate { get; set; }
    }
}
