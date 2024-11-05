using e_commerce.Models.Entities;
using e_commerce.Models.Entities.RefEntities;
using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.DTOs.ProductDTOs
{
    public class ReadProductDTO
    {
        public int ProductID { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string SmallDescription { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }
        public int StockQuantity { get; set; }
        public List<ProductImage>? ProductImages { get; set; }

        public string SellerID { get; set; }
        public string SellerName { get; set; }
        //public SellerDTO Seller { get; set; }
        public RefProductTypeDTO RefProductType { get; set; }
    }
}
