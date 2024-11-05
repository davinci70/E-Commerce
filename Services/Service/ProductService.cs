using e_commerce.Data;
using e_commerce.Models.DTOs.ProductDTOs;
using e_commerce.Models.Entities;
using e_commerce.Models.Entities.RefEntities;
using e_commerce.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _Context;
        private readonly CloudinaryService _cloudinaryService;

        public ProductService(AppDbContext Context, CloudinaryService cloudinaryService)
        {
            _Context = Context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task AddAsync(ProductDTO model)
        {
            var newProduct = new Product()
            {
                Description = model.Description,
                SmallDescription = model.SmallDescription,
                Name = model.Name,
                ProductTypeID = model.ProductTypeID,
                Price = model.Price,
                Discount = model.Discount,
                StockQuantity = model.StockQuantity,
                SellerID = model.SellerID
            };

            if (model.ProductImages != null)
            {
                foreach (var image in model.ProductImages)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                    newProduct.ProductImages.Add(new ProductImage { ImageUrl = imageUrl });
                }
            }

           
            await _Context.Products.AddAsync(newProduct);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ValidationException("Id is not valid.");
            }

            var product = await _Context.Products
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.ProductID == ID);

            if (product == null)
            {
                throw new InvalidOperationException($"Product with Id {ID} not found.");
            }

            using (var transaction = _Context.Database.BeginTransaction())
            {
                try
                {
                    if (product.ProductImages.Any())
                    {
                        _Context.ProductImages.RemoveRange(product.ProductImages);
                        _Context.Products.Remove(product);
                    }
                   
                    await _Context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch 
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException("Failed to delete product.");
                }
            }
        }

        public async Task<ICollection<ReadProductDTO>> GetAllAsync()
        {
            var products = await _Context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ProductImages)
                .Include(x => x.Seller)
                .ToListAsync();

            if (!products.Any())
            {
                throw new InvalidOperationException("No products found.");
            }

            var productDTOs = products.Select(product => new ReadProductDTO
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                SmallDescription = product.SmallDescription,
                Price = product.Price,
                Discount = product.Discount,
                StockQuantity = product.StockQuantity,
                ProductTypeID = product.ProductTypeID,
                ProductTypeName = product.ProductType.ProductTypeName,
                SellerID = product.SellerID,
                SellerName = product.Seller.FirstName + " " + product.Seller.LastName,
                ProductImages = product.ProductImages.Select(img => new ProductImage
                {
                    ProductImageID = img.ProductImageID,
                    ImageUrl = img.ImageUrl
                }).ToList()
            }).ToList();

            return productDTOs;
        }

        public async Task<ReadProductDTO> GetByIdAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ValidationException("Id is not valid.");
            }

            var product = await _Context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ProductImages)
                .Include(x => x.Seller)
                .FirstOrDefaultAsync(x => x.ProductID == ID);

            if (product == null)
            {
                throw new InvalidOperationException($"Product with Id {ID} not found.");
            }

            var productDTO = new ReadProductDTO()
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                SmallDescription = product.SmallDescription,
                Price = product.Price,
                Discount = product.Discount,
                StockQuantity = product.StockQuantity,
                ProductTypeID = product.ProductTypeID,
                SellerID = product.SellerID,
                ProductImages = product.ProductImages.Select(img => new ProductImage
                {
                    ProductImageID = img.ProductImageID,
                    ImageUrl = img.ImageUrl
                }).ToList()
            };

            return productDTO;
        }

        public async Task UpdateAsync(int ID, UpdateProductDTO model)
        {
            if (ID <= 0)
            {
                throw new ValidationException("Id is not valid.");
            }

            var product = await _Context.Products
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.ProductID == ID);

            if (product == null)
            {
                throw new InvalidOperationException($"Product with Id {ID} not found.");
            }

            product.ProductTypeID = model.ProductTypeID;
            product.Name = model.Name;
            product.Description = model.Description;
            product.SmallDescription = model.SmallDescription;
            product.Discount = model.Discount;
            product.StockQuantity = model.StockQuantity;
            product.Price = model.Price;
            product.SellerID = model.SellerID;

            if (model.RemoveImagesIDs != null && model.RemoveImagesIDs.Any())
            {
                foreach (var imageID in model.RemoveImagesIDs)
                {
                    var image = product.ProductImages.FirstOrDefault(x => x.ProductImageID == imageID);

                    if (image != null)
                    {                       
                        await _cloudinaryService.DeleteImageAsync(image.ImageUrl);
                        _Context.ProductImages.Remove(image);
                    }
                }
            }

            if (model.ProductImages != null && model.ProductImages.Any())
            {
                foreach (var newImage in model.ProductImages)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(newImage);
                    product.ProductImages.Add(new ProductImage { ImageUrl = imageUrl });
                }
            }

            _Context.Products.Update(product);
            await _Context.SaveChangesAsync();
        }
    }
}
