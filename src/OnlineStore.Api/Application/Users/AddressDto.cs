using System;
using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Application.Users
{
    public class AddressDto : Dto<Address>
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string HouseNumber { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }


        public AddressDto()
        {
        }

        public AddressDto(Address entity)
        {
            Street = entity.Street;
            City = entity.City;
            PostalCode = entity.PostalCode;
            HouseNumber = entity.HouseNumber;
            UserId = entity.UserId;
        }

        public override Address ToEntity(Address entity = null)
        {
            entity ??= new Address();

            entity.Id = Id;
            entity.Street = Street;
            entity.City = City;
            entity.PostalCode = PostalCode;
            entity.HouseNumber = HouseNumber;
            entity.UserId = UserId;

            return entity;
        }
    }
}

