using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Domain.Orders;
using System;

namespace OnlineStore.Api.Application.Orders
{
    public class ReviewDto : Dto<Review>
    {
        public string Feedback { get; set; }

        public double Rating { get; set; }

        public Guid ProductId { get; set; }
        public virtual ProductDto Product { get; set; }

        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; }


        public ReviewDto(Review entity)
        {
            Id = entity.Id;
            Feedback = entity.Feedback;
            Rating = entity.Rating;
            ProductId = entity.ProductId;
            UserId = entity.UserId;

            if (entity.Product != null)
            {
                Product = new ProductDto(entity.Product);
            }
        }

        public ReviewDto()
        {

        }

        public override Review ToEntity(Review entity = null)
        {
            entity ??= new Review();

            entity.Id = Id;
            entity.Feedback = Feedback;
            entity.Rating = Rating;
            entity.ProductId = ProductId;
            entity.UserId = UserId;

            return entity;
        }
    }
}

