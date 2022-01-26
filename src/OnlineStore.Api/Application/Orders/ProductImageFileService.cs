using OnlineStore.Api.Application.Common;
using OnlineStore.Api.Application.Orders.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Azure;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Orders
{
    public class ProductImageFileService : IProductImageFileService
    {
        private readonly IBlobStorage _blobStorage;
        private readonly IRepository<Product> _productRepository;

        public ProductImageFileService(IBlobStorage blobStorage, IRepository<Product> productRepository)
        {
            _blobStorage = blobStorage;
            _productRepository = productRepository;
        }


        public async Task SaveAsync(Guid productId, FileData fileData, CancellationToken cancellationToken = default)
        {
            var path = _blobStorage.GetFilePath(StorageDataType.Product, productId, fileData.Filename);
            await _blobStorage.SaveFileAsync(path, fileData.FileStream, cancellationToken);

            await SetDocumentPathAsync(productId, path, cancellationToken);
        }

        public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var path = await GetDocumentPathAsync(productId, cancellationToken);
            if (path != string.Empty)
            {
                await _blobStorage.DeleteFileAsync(path, cancellationToken);
            }
            await SetDocumentPathAsync(productId, string.Empty, cancellationToken);
        }

        public async Task<FileData> GetAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var filePath = await GetDocumentPathAsync(productId, cancellationToken);
            var fileStream = await _blobStorage.GetFileAsync(filePath, cancellationToken);
            var filename = Path.GetFileName(filePath);
            return new FileData { Filename = filename, FileStream = fileStream };
        }

        private async Task<Product> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.FindOneAsync(new ProductWithId(productId), cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            return product;
        }

        private async Task<string> GetDocumentPathAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var product = await GetProductAsync(productId, cancellationToken);
            return product.ImageFilePath;
        }

        private async Task SetDocumentPathAsync(Guid productId, string filePath, CancellationToken cancellationToken = default)
        {
            var product = await GetProductAsync(productId, cancellationToken);
            product.ImageFilePath = filePath;
            await _productRepository.SaveAsync(product, cancellationToken);
        }
    }
}
