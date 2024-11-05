using e_commerce.Data;
using e_commerce.Models.DTOs.ProductDTOs;
using e_commerce.Models.Entities;
using e_commerce.Models.Entities.RefEntities;
using e_commerce.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Services.Service
{
    public class RefProductTypeService : IRefProductTypeService
    {
        private readonly AppDbContext _context;

        public RefProductTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefProductTypeDTO model)
        {
            var productionType = new RefProductType()
            {
                ProductTypeName = model.ProductTypeName
            };

            await _context.RefProductTypes.AddAsync(productionType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Id is not valid.");
            }

            var refProductType = await _context.RefProductTypes
                .FirstOrDefaultAsync(x => x.ProductTypeID == ID);

            if (refProductType == null)
            {
                throw new InvalidOperationException("Product type not found.");
            }

            _context.RefProductTypes.Remove(refProductType);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RefProductType>> GetAllAsync()
        {
            var productTypes = await _context.RefProductTypes
                .Select(x => new RefProductType
                {
                    ProductTypeID = x.ProductTypeID,
                    ProductTypeName = x.ProductTypeName,
                    //Products = x.Products
                }).ToListAsync();

            if (!productTypes.Any())
            {
                throw new InvalidOperationException("No product types found.");
            }

            return productTypes;
        }

        public async Task<RefProductType> GetByIdAsync(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Id is not valid.");
            }

            var refProductType = await _context.RefProductTypes.FindAsync(ID);

            if (refProductType == null)
            {
                throw new InvalidOperationException("Production type not found.");
            }

            return refProductType;
        }

        public async Task UpdateAsync(int ID, RefProductTypeDTO model)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Id is not valid.");
            }

            var refProductType = await _context.RefProductTypes
                .FirstOrDefaultAsync(x => x.ProductTypeID == ID);

            if (refProductType == null)
            {
                throw new InvalidOperationException("Production type not found.");
            }

            refProductType.ProductTypeName = model.ProductTypeName;

            await _context.SaveChangesAsync();
        }
    }
}
