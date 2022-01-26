using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.Azure.Interfaces
{
    public interface IBlobStorage
    {
        Task SetupContainersAsync();
        Uri GetUrl(string path);
        Uri GetUrl(string path, TimeSpan offset);
        Task<Stream> GetFileAsync(string path, CancellationToken cancellationToken = default);
        Task<bool> FileExistsAsync(string path, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);
        Task<string[]> GetFilesAsync(string path, CancellationToken cancellationToken = default);
        Task SaveFileAsync(string path, Stream stream, CancellationToken cancellationToken = default);
        string GetFilePath(StorageDataType storageType, Guid id, string filename, string subfolder = null);
    }
}
