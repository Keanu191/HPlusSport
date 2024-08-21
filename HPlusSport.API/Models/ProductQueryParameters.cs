namespace HPlusSport.API.Models
{
    public class ProductQueryParameters : QueryParameters // implement pagination query parameters
    {
        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Sku { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

    }
}
