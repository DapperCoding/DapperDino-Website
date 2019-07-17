using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("/Admin/ProductImages")]
    public class ProductImageController : BaseController
    {
        #region Fields

        private ApplicationDbContext _context;
        private IConfiguration _configuration;

        #endregion

        public ProductImageController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Route("Edit/{productImageId}")]
        public async Task<IActionResult> Edit(int productImageId)
        {

            var productImage = await _context.ProductImages.SingleOrDefaultAsync(x => x.Id == productImageId);

            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        [HttpPost("Edit/{productImageId}")]
        public async Task<IActionResult> Edit(int productImageId, ProductImage newImage)
        {

            var productImage = await _context.ProductImages.SingleOrDefaultAsync(x => x.Id == productImageId);

            if (productImage == null)
            {
                return NotFound();
            }

            productImage.Alt = newImage.Alt;
            productImage.Description = newImage.Description;
            productImage.IsHeaderImage = newImage.IsHeaderImage;
            productImage.Name = newImage.Name;

            return View(productImage);
        }

        [Route("Delete/{productImageId}")]
        public async Task<IActionResult> Delete(int productImageId)
        {
            var productImage = await _context.ProductImages.SingleOrDefaultAsync(x => x.Id == productImageId);

            if (productImage == null)
            {
                return NotFound();
            }

            // TODO Delete the image from blob storage

            _context.Remove(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Images","Product", new { productId = productImage.ProductId});
        }

        [HttpPost]
        //OPTION A: Disables Asp.Net Core's default upload size limit
        [DisableRequestSizeLimit]
        //OPTION B: Uncomment to set a specified upload file limit
        //[RequestSizeLimit(40000000)] 
        [Route("{productImageId}/replace")]
        public async Task<IActionResult> PostFiles(int productImageId, IFormFile formFile)
        {
            var uploadSuccess = false;
            string uploadedUri = null;

            var productImage = await _context.ProductImages.SingleOrDefaultAsync(x => x.Id == productImageId);

            // Cannot find product
            if (productImage == null)
            {
                return NotFound();
            }

            if (formFile.Length <= 0)
            {
                return BadRequest();
            }

            using (var stream = formFile.OpenReadStream())
            {
                (uploadSuccess, uploadedUri) = await UploadToBlob(productImage.ProductId, formFile.FileName, stream);

                if (uploadSuccess)
                {
                    productImage.Url = uploadedUri;
                    _context.SaveChanges();
                }
            }


            return RedirectToAction("Images", "Product", new { productId = productImage.ProductId });
        }

        private async Task<(bool, string)> UploadToBlob(int productId, string filename, Stream stream = null)
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
            string storageConnectionString = _configuration["storageconnectionstring"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'uploadblob' and append a GUID value to it to make the name unique. 
                    cloudBlobContainer = cloudBlobClient.GetContainerReference($"product{productId}");
                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

                    if (stream != null)
                    {
                        // OPTION B: pass in memory stream directly
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return (false, null);
                    }

                    return (true, cloudBlockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString());
                }
                catch (StorageException ex)
                {
                    return (false, null);
                }
                finally
                {
                    // OPTIONAL: Clean up resources, e.g. blob container
                    //if (cloudBlobContainer != null)
                    //{
                    //    await cloudBlobContainer.DeleteIfExistsAsync();
                    //}
                }
            }
            else
            {
                return (false, null);
            }

        }
    }
}
