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
        void AddItemToBasket(int baksetId, int catalogItemId, int quantity);
        bool RemoveItemFromBasket(int ItemId);
        bool SetQuantities(int itemId, int quantity);
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

        public void AddItemToBasket(int baksetId, int catalogItemId, int quantity)
        {
            var basket = context.Baskets.FirstOrDefault(b => b.Id == baksetId);
            if (basket == null)
            {
                throw new Exception("");
            }
            var catalogItem = context.CatalogItems.Find(catalogItemId);
            basket.AddItem(catalogItem.Price, quantity, catalogItemId);
            context.SaveChanges();
        }

        public BasketDto GetOrCreateBasketForUser(string buyerId)
        {
            var basket = context.Baskets
               .Include(p => p.Items)
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
                Items = basket.Items.Select(item => new BasketItemDto
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

        public bool RemoveItemFromBasket(int ItemId)
        {
            var item = context.BasketItems.SingleOrDefault(b => b.Id == ItemId);
            context.BasketItems.Remove(item);
            context.SaveChanges();
            return true;
        }

        public bool SetQuantities(int itemId, int quantity)
        {
            var item = context.BasketItems.SingleOrDefault(p => p.Id == itemId);
            item.SetQuantity(quantity);
            context.SaveChanges();
            return true;
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
