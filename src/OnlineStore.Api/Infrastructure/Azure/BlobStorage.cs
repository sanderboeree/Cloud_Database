using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Infrastructure.Extensions;

namespace OnlineStore.Api.Infrastructure.Azure
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        private const string FileDirectory = "files";

        public BlobStorage(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task SetupContainersAsync()
        {
            await _blobServiceClient.GetBlobContainerClient(FileDirectory).CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        public Uri GetUrl(string path)
        {
            return GetUrl(path, TimeSpan.FromMinutes(10));
        }

        public Uri GetUrl(string path, TimeSpan offset)
        {
            var containerName = GetRootDirectory(path);
            var blobPath = GetBlobPath(containerName, path);
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = container.GetBlobClient(blobPath);

            var credential = new StorageSharedKeyCredential(GetConnectionStringValue("AccountName"), GetConnectionStringValue("AccountKey"));
            var sas = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTime.UtcNow.AddMinutes(-1),
                ExpiresOn = DateTime.UtcNow.Add(offset)
            };
            sas.SetPermissions(BlobSasPermissions.Read);
            var blobEndpoint = GetConnectionStringValue("BlobEndpoint");
            var sasUri = new UriBuilder($"{blobEndpoint}/{blobClient.BlobContainerName}/{blobClient.Name}")
            {
                Query = sas.ToSasQueryParameters(credential).ToString()
            };

            return sasUri.Uri;
        }

        public async Task<Stream> GetFileAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var blobClient = GetBlobClient(path);
            var memStream = new MemoryStream();
            await blobClient.DownloadToAsync(memStream, cancellationToken);
            memStream.Position = 0;

            return memStream;
        }

        public async Task<bool> FileExistsAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var blobClient = GetBlobClient(path);
            return await blobClient.ExistsAsync(cancellationToken);
        }

        public async Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }


            var blobClient = GetBlobClient(path);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<string[]> GetFilesAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var containerName = GetRootDirectory(path);
            var blobPath = GetBlobPath(containerName, path);

            var container = GetBlobContainerClient(containerName);

            var blobs = container.GetBlobsAsync(prefix: blobPath, cancellationToken: cancellationToken);
            var listOfFileNames = new List<string>();

            await foreach (var blob in blobs)
            {
                var blobClient = container.GetBlobClient(blob.Name);
                if (blobClient != null && !blobClient.Name.Contains("resized"))
                {
                    listOfFileNames.Add($"{containerName}/{blobClient.Name}");
                }
            }

            return listOfFileNames.ToArray();
        }

        public async Task SaveFileAsync(string path, Stream stream, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var blobClient = GetBlobClient(path);

            stream.Position = 0;
            await blobClient.UploadAsync(stream, true, cancellationToken);

            // Default contenttype in blobstorage = application/octet-stream, this works for all files except for svg (browser required image/svg+xml contenttype to display svg)
            if (path.ToLower().EndsWith(".svg"))
            {
                await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = "image/svg+xml" }, cancellationToken: cancellationToken);
            }

        }

        public string GetFilePath(StorageDataType storageType, Guid id, string filename, string subfolder = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var domainName = storageType switch
            {
                StorageDataType.Product => "product",
                _ => "unknown"
            };

            return Path.Combine(FileDirectory, domainName, id.ToString(), subfolder ?? string.Empty, filename.SafeUrl());
        }

        private BlobClient GetBlobClient(string storePath)
        {
            var containerName = GetRootDirectory(storePath);
            var blobPath = GetBlobPath(containerName, storePath);

            var container = GetBlobContainerClient(containerName);

            return container.GetBlobClient(blobPath);
        }

        private BlobContainerClient GetBlobContainerClient(string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);

            return container;
        }

        private static string GetRootDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);

            while (!string.IsNullOrWhiteSpace(dir))
            {
                var parent = Path.GetDirectoryName(dir);
                if (string.IsNullOrWhiteSpace(parent))
                {
                    return dir;
                }

                dir = parent;
            }

            throw new ArgumentException("Path should have a root directory");
        }

        private static string GetBlobPath(string containerName, string path)
        {
            var blobPath = path.Substring(containerName.Length).Replace(@"\", "/");

            while (blobPath.Substring(0, 1) == "/")
            {
                blobPath = blobPath.Substring(1);
            }

            while (blobPath.Substring(0, 1) == @"\")
            {
                blobPath = blobPath.Substring(1);
            }

            return blobPath;
        }

        private string GetConnectionStringValue(string key)
        {
            var kvp = _configuration.GetConnectionString("AzureStorage").Split(';', StringSplitOptions.RemoveEmptyEntries);

            return kvp.Where(value => value.StartsWith(key)).Select(value => value.Substring(key.Length + 1)).FirstOrDefault();
        }
    }
}
