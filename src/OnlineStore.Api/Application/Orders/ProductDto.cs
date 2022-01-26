using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Application.Orders
{
    public class ProductDto : Dto<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string ImageFilePath { get; set; }

        public ProductDto()
        {
        }

        public ProductDto(Product entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Price = entity.Price;
            Description = entity.Description;
            Specification = entity.Specification;
            ImageFilePath = !string.IsNullOrWhiteSpace(entity.ImageFilePath) ? $"v1/products/{entity.Id}/image" : string.Empty;
        }

        public override Product ToEntity(Product entity = null)
        {
            entity ??= new Product();

            entity.Id = Id;
            entity.Name = Name;
            entity.Price = Price;
            entity.Description = Description;
            entity.Specification = Specification;

            return entity;
        }
    }
}