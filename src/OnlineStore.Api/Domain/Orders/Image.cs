using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Api.Domain.Orders
{
    public class Image : Entity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
