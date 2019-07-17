using DapperDino.Areas.Admin.Models.Products;
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
    [Route("Admin/Product")]
    public class ProductController : BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor(s)

        public ProductController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #endregion

        [Route("")]
        public IActionResult Index()
        {
            // Get list of all products
            var products = _context.Products.Include(x => x.Categories).Include(x => x.Instructions).ToList();

            // Generate IndexViewModel using the products list
            var viewModel = new IndexViewModel()
            {

                Products = products
            };

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("Add")]
        public IActionResult Add()
        {
            // Get list of all products
            var viewModel = new ProductEditViewModel();

            viewModel.AllCategories = _context.ProductCategories
                .Select(x => new ProductProductCategory() { ProductCategory = x, ProductCategoryId = x.Id })
                .ToList();

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            // Get list of all products
            var product = _context.Products.Include(x => x.Categories).Include(x => x.Instructions).FirstOrDefault(x => x.Id == id);

            // Does the product exist?
            if (product == null) return NotFound("Product can't be found");

            var viewModel = new ProductEditViewModel();

            viewModel.Description = product.Description;
            viewModel.Id = product.Id;
            viewModel.ShortDescription = product.ShortDescription;
            viewModel.SalePercentage = product.SalePercentage;
            viewModel.Price = product.Price;
            viewModel.Categories = product.Categories;
            viewModel.Name = product.Name;
            viewModel.IsActive = product.IsActive;

            viewModel.AllCategories = _context.ProductCategories
                .Select(x => new ProductProductCategory() { ProductCategory = x, ProductCategoryId = x.Id, Product = product, ProductId = product.Id })
                .ToList();

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(ProductEditViewModel viewModel)
        {
            Product product;

            if (viewModel.Id > 0)
            {
                product = await _context.Products.Include(x=>x.Categories).SingleOrDefaultAsync(x=>x.Id == viewModel.Id);

                if (product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                product = new Product();
                _context.Products.Add(product);
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.ShortDescription = viewModel.ShortDescription;
            product.Price = viewModel.Price;
            product.SalePercentage = viewModel.SalePercentage;
            product.IsActive = viewModel.IsActive;

            if (viewModel.Id <= 0)
            {
                await _context.SaveChangesAsync();
            }

            if (product.Categories == null )
            {
                product.Categories = new List<ProductProductCategory>();
            }


            if (viewModel.SelectedProductCategories != null)
            {
                if (product.Categories.Count > 0)
                    product.Categories.RemoveAll(x => true);

                foreach (var cat in viewModel.SelectedProductCategories)
                {
                    product.Categories.Add(new ProductProductCategory() { ProductId = product.Id, ProductCategoryId = cat });
                }

                await _context.SaveChangesAsync();
            }



            // Return the view -> using viewModel
            return RedirectToAction("Get", new { id = product.Id });
        }


        [Route("{productId}/AddImages")]
        public async Task<IActionResult> AddImages(int productId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var model = new AddImagesViewModel();
            model.ProductId = productId;

            return View(model);
        }

        [Route("{productId}/Images")]
        public async Task<IActionResult> Images(int productId)
        {
            var product = await _context.Products.Include(x=>x.ProductImages).SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ImagesOverviewViewModel();

            model.ProductId = productId;
            model.Images = product.ProductImages;

            return View(model);
        }


        [HttpPost]
        //OPTION A: Disables Asp.Net Core's default upload size limit
        [DisableRequestSizeLimit]
        //OPTION B: Uncomment to set a specified upload file limit
        //[RequestSizeLimit(40000000)] 
        [Route("{productId}/UploadFiles")]
        public async Task<IActionResult> PostFiles(int productId, List<IFormFile> files)
        {
            var uploadSuccess = false;
            string uploadedUri = null;
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == productId);

            // Cannot find product
            if (product == null)
            {
                return NotFound();
            }

            foreach (var formFile in files)
            {
                if (formFile.Length <= 0)
                {
                    continue;
                }

                // NOTE: uncomment either OPTION A or OPTION B to use one approach over another

                // OPTION A: convert to byte array before upload
                //using (var ms = new MemoryStream())
                //{
                //    formFile.CopyTo(ms);
                //    var fileBytes = ms.ToArray();
                //    uploadSuccess = await UploadToBlob(formFile.FileName, fileBytes, null);

                //}

                // OPTION B: read directly from stream for blob upload      
                using (var stream = formFile.OpenReadStream())
                {
                    (uploadSuccess, uploadedUri) = await UploadToBlob(productId, formFile.FileName, null, stream);

                    if (uploadSuccess)
                    {
                        var image = new ProductImage()
                        {
                            Name = formFile.Name,
                            Url = uploadedUri,
                            Alt = "Product image",
                            Description = "Product image",
                            ProductId = productId
                        };

                        _context.ProductImages.Add(image);
                        _context.SaveChanges();
                    }

                    TempData["uploadedUri"] = uploadedUri;
                }

            }

            if (uploadSuccess)
                return RedirectToAction("Images", new { productId });
            else
                return RedirectToAction("AddImages", new { productId });
        }

        private async Task<(bool, string)> UploadToBlob(int productId, string filename, byte[] imageBuffer = null, Stream stream = null)
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

                    if (imageBuffer != null)
                    {
                        // OPTION A: use imageBuffer (converted from memory stream)
                        await cloudBlockBlob.UploadFromByteArrayAsync(imageBuffer, 0, imageBuffer.Length);
                    }
                    else if (stream != null)
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