using OnlineStore.Api.Domain.Orders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Api.Application.Orders
{
    public class ImageDto : Dto<Image>
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public ImageDto()
        {
        }

        public ImageDto(Image entity)
        {
            Name = entity.Name;
            ContentType = entity.ContentType;
            Content = entity.Content;
            ProductId = entity.ProductId;
        }

        public override Image ToEntity(Image entity = null)
        {
            entity ??= new Image();

            entity.Id = Id;
            entity.Name = Name;
            entity.ContentType = ContentType;
            entity.Content = Content;
            entity.ProductId = ProductId;

            return entity;
        }
    }
}

