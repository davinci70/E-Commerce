using e_commerce.Data;
using e_commerce.Helpers;
using e_commerce.Models.DTOs.OrderDTOs;
using e_commerce.Models.Entities;
using e_commerce.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Service
{

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _Context;

        public OrderService(AppDbContext Context)
        {
            _Context = Context;            
        }

        public async Task AddAsync(OrderDTO model)
        {
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in model.OrderItems)
                    {
                        var product = await _Context.Products.FindAsync(item.ProductID);

                        if (product == null)
                        {
                            throw new Exception($"Product with ID {item.ProductID} is not found.");
                        }

                        var itemTotal = product.Price * item.Quantity;
                        totalAmount += itemTotal;

                        product.StockQuantity -= item.Quantity;

                        var orderItem = new OrderItem
                        {
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            ItemPrice = product.Price
                        };

                        orderItems.Add(orderItem);
                    }

                    var order = new Order
                    {
                        CustomerID = model.CustomerID,
                        OrderDate = DateTime.Now,
                        ShippingAddress = model.ShippingAddress,
                        TotalPrice = totalAmount,
                        Status = OrderStatus.enOrderStatus.Pending.ToString(),
                        OrderItems = orderItems
                    };

                    _Context.Orders.Add(order);
                    await _Context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to place order.");
                }
            }
        }

        public async Task<ICollection<OrderDTO>> GetCustomerOrdersAsync(string CustomerID)
        {
            if (string.IsNullOrWhiteSpace(CustomerID))
            {
                throw new ArgumentException("Customer ID is not valid.");
            }

            var orders = await _Context.Orders
                .Include(oi => oi.OrderItems)
                .Where(x => x.CustomerID == CustomerID)
                .ToListAsync();

            if (!orders.Any())
            {
                throw new ArgumentException("No orders here.");
            }

            var ordersDTO = orders.Select(order => new OrderDTO
            {
                CustomerID = order.CustomerID,
                ShippingAddress = order.ShippingAddress,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductID = oi.ProductID,
                    Quantity = oi.Quantity
                }).ToList()

            }).ToList();

            return ordersDTO;
        }

        public async Task UpdateAsync(int ID, UpdateOrderDTO model)
        {
            if (ID <= 0)
            {
                throw new ValidationException("Order ID is invalid.");
            }

            var Order = await _Context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.OrderID == ID);

            if (Order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            if (Order.Status == "Shipped" || Order.Status == "Delivered")
            {
                throw new ValidationException("Cannot update an order that has already been shipped or delivered.");
            }

            if (Order.Status == "Cancelled")
            {
                throw new ValidationException("Cannot update a cancelled order.");
            }

            decimal totalAmount = 0;

            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (Order.OrderItems != null)
                    {
                        Order.OrderItems.Clear();

                        foreach (var item in model.OrderItems)
                        {
                            var product = await _Context.Products.FindAsync(item.ProductID);

                            if (product == null)
                            {
                                throw new InvalidOperationException($"Product with ID {item.ProductID} is not found.");
                            }

                            var itemTotal = product.Price * item.Quantity;
                            totalAmount += itemTotal;

                            var orderItem = new OrderItem
                            {
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                ItemPrice = product.Price
                            };

                            Order.OrderItems.Add(orderItem);
                        }
                    }

                    Order.ShippingAddress = model.ShippingAddress;
                    Order.TotalPrice = totalAmount;

                    await _Context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new ValidationException("Failed to update order");
                }
            }
        }
        public async Task UpdateOrderStatusAsync(int OrderID, OrderStatus.enOrderStatus orderStatus)
        {
            if (OrderID <= 0)
            {
                throw new ValidationException("Order ID is invalid.");
            }

            var Order = await _Context.Orders.FindAsync(OrderID);

            if (Order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            Order.Status = orderStatus.ToString();
            await _Context.SaveChangesAsync();
        }
    }
}
