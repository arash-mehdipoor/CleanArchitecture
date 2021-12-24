using Application.Catalogs.CatalogItems.UriComposer;
using Application.Interfaces.Context;
using Domain.Baskets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BasketService
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string buyerId);
    }

    public class BasketService : IBasketService
    {
        private readonly IDatabaseContext context;
        private readonly IUriComposerService uriComposerService;

        public BasketService(IDatabaseContext context
            , IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
        }

        public BasketDto GetOrCreateBasketForUser(string buyerId)
        {
            var basket = context.Baskets
               .Include(p => p.BasketItems)
               .ThenInclude(p => p.CatalogItem)
               .ThenInclude(p => p.CatalogItemImages)
               .SingleOrDefault(p => p.BuyerId == buyerId);
            if (basket == null)
            {
                return CreateBasketForUser(buyerId);
            }
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.BasketItems.Select(item => new BasketItemDto
                {
                    CatalogItemId = item.CatalogItemId,
                    Id = item.Id,
                    CatalogName = item.CatalogItem.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ImageUrl = uriComposerService.ComposeImageUri(item?.CatalogItem?
                     .CatalogItemImages?.FirstOrDefault()?.Src ?? ""),

                }).ToList(),
            };

        }
        private BasketDto CreateBasketForUser(string BuyerId)
        {
            Basket basket = new Basket(BuyerId);
            context.Baskets.Add(basket);
            context.SaveChanges();
            return new BasketDto
            {
                BuyerId = basket.BuyerId,
                Id = basket.Id,
            };
        }
    }
    public class BasketDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

    }

    public class BasketItemDto
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }
        public string CatalogName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
