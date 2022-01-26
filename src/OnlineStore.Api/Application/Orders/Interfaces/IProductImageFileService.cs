using OnlineStore.Api.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Orders.Interfaces
{
    public interface IProductImageFileService
    {
        Task SaveAsync(Guid productId, FileData fileData, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<FileData> GetAsync(Guid productId, CancellationToken cancellationToken = default);

    }
}
