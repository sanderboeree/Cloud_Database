namespace OnlineStore.Api.Domain.Orders
{
    public class Product : SoftDeleteEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string ImageFilePath { get; set; }
    }
}
