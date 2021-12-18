using Domain.Attributes;

namespace Domain.Catalogs
{
    [Audit]
    public class CatalogItemFeature
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }

        public int CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
    }
}
