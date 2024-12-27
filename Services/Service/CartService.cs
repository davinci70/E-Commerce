using e_commerce.Data;
using e_commerce.Models.DTOs;
using e_commerce.Models.DTOs.CartDTOs;
using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using e_commerce.Services.IService;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Service
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _Context;

        public CartService(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task AddOrUpdateCartAsync(CartDTO model)
        {
            // Validate Customer
            var customer = await _Context.Customers.FirstOrDefaultAsync(x => x.Id == model.CustomerID);
            if (customer == null)
            {
                throw new ArgumentException("Customer ID is not valid.");
            }

            // Validate Product
            var product = await _Context.Products.FirstOrDefaultAsync(x => x.ProductID == model.ProductID);
            if (product == null)
            {
                throw new ArgumentException("Product ID is not valid.");
            }

            // Find or Create Cart
            var cart = await _Context.Carts
                .Include(x => x.cartItems)
                .FirstOrDefaultAsync(x => x.CustomerID == model.CustomerID);

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerID = model.CustomerID,
                    cartItems = new List<CartItem>()
                };

                await _Context.Carts.AddAsync(cart);
            }

            // Find CartItem
            var cartItem = cart.cartItems.FirstOrDefault(x => x.ProductID == model.ProductID);

            // Calculate the new quantity
            int newQuantity = cartItem != null ? cartItem.Quantity + model.Quantity : model.Quantity;

            if (newQuantity <= 0)
            {
                // Remove the item if quantity becomes zero or less
                if (cartItem != null)
                {
                    cart.cartItems.Remove(cartItem);
                }
            }
            else
            {
                // Validate Stock Quantity
                if (newQuantity > product.StockQuantity)
                {
                    throw new ArgumentException($"Requested quantity ({newQuantity}) exceeds available stock ({product.StockQuantity}).");
                }

                if (cartItem != null)
                {
                    // Update the quantity of the existing cart item
                    cartItem.Quantity = newQuantity;
                }
                else
                {
                    // Add a new cart item
                    cart.cartItems.Add(new CartItem
                    {
                        ProductID = model.ProductID,
                        Quantity = model.Quantity,
                        Price = product.Price
                    });
                }
            }

            // Save changes
            await _Context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string customerId)
        {
            var cart = await _Context.Carts
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                throw new ArgumentException("Cart not found for the specified customer.");
            }

            _Context.Carts.Remove(cart);
            await _Context.SaveChangesAsync();
        }

        public async Task<ReadCartDTO> GetCartByCustomerID(string customerID)
        {
            var Cart = await _Context.Carts
                .Include(ci => ci.cartItems)
                .Where(c => c.CustomerID == customerID)
                .Select(dto => new ReadCartDTO
                {
                    CustomerID = dto.CustomerID,
                    CartId = dto.CartId,
                    cartItems = dto.cartItems.Select(c => new ReadCartItemDTO
                    {
                        ProductName = c.Product.Name,
                        ProductID = c.ProductID,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        Description = c.Product.Description,
                        ProductImages = c.Product.ProductImages.Select(img => new ProductImage
                        {
                            ImageUrl = img.ImageUrl
                        }).ToList()

                    }).ToList()
                }
                ).FirstOrDefaultAsync();
            
            if (Cart == null)
            {
                throw new ArgumentException("Cart is empty.");
            }

            return Cart;
        }

        public async Task MoveAllCartAsync(CartListDTO carList)
        {
            var customer = await _Context.Customers.FirstOrDefaultAsync(x => x.Id == carList.CustomerID);

            if (customer == null)
            {
                throw new ArgumentException("Customer ID is not valid.");
            }

            var cart = await _Context.Carts
                .Include(x => x.cartItems)
                .FirstOrDefaultAsync(x => x.CustomerID == carList.CustomerID);

            if (cart == null)
            {
                cart = new Cart()
                {
                    CustomerID = carList.CustomerID,
                    cartItems = new List<CartItem>()
                };

                await _Context.Carts.AddAsync(cart);
            }

            foreach (var item in carList.CartList)
            {
                var cartItem = cart.cartItems
                    .FirstOrDefault(x => x.ProductID == item.ProductID);

                if (cartItem != null)
                {
                    cartItem.Quantity += item.Quantity;
                }
                else
                {
                    var product = await _Context.Products.FindAsync(item.ProductID);

                    if (product == null)
                    {
                        throw new ArgumentException($"Invalid Product ID: {item.ProductID}");
                    }

                    cart.cartItems.Add(new CartItem()
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            await _Context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string customerId, int productId)
        {
            var cart = await _Context.Carts
                .Include(c => c.cartItems)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (cart == null)
            {
                throw new ArgumentException("Cart not found for the specified customer.");
            }


            var cartItem = cart.cartItems.FirstOrDefault(ci => ci.ProductID == productId);

            if (cartItem == null)
            {
                throw new ArgumentException("Product not found in the cart.");
            }

            cart.cartItems.Remove(cartItem);
            await _Context.SaveChangesAsync();
        }
    }
}
