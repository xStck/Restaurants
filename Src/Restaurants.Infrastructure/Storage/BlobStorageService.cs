using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettings.Value;
    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        var blobStorageClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobStorageClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);
        var blobUrl = blobClient.Uri.ToString();
        return blobUrl; 
    }
}  