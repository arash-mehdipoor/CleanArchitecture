using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Baskets
{
    [Audit]
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; private set; }
        private readonly List<BasketItem> _items = new List<BasketItem>();

        public Basket(string buyerId)
        {
            this.BuyerId = buyerId;
        }
        public ICollection<BasketItem> Items => _items.AsReadOnly();

        public void AddItem(int unitPrice, int quantity, int catalogItemId)
        {
            if (!_items.Any(b => b.CatalogItemId == catalogItemId))
            {
                _items.Add(new BasketItem(unitPrice, quantity, catalogItemId));
            }
            var existsItem = _items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
            existsItem.AddQuantity(quantity);
        }
    }
}
