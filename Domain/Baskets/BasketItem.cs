using Domain.Attributes;
using Domain.Catalogs;

namespace Domain.Baskets
{
    [Audit]
    public class BasketItem
    {
        public int Id { get; set; }
        public int UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public int BasketId { get; private set; }

        public int CatalogItemId { get; private set; }
        public CatalogItem CatalogItem { get; private set; }

        public BasketItem(int unitPrice, int quantity, int catalogItemId)
        {
            UnitPrice = unitPrice;
            Quantity = quantity;
            CatalogItemId = catalogItemId;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
