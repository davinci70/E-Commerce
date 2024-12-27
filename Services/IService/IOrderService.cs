using e_commerce.Helpers;
using e_commerce.Models.DTOs.OrderDTOs;
using e_commerce.Models.Entities;

namespace e_commerce.Services.IService
{
    public interface IOrderService
    {
        public Task<int> AddAsync(OrderDTO model);
        public Task UpdateAsync(int ID, UpdateOrderDTO model);
        public Task UpdateOrderStatusAsync(int OrderID, OrderStatus.enOrderStatus orderStatus);
        public Task<ICollection<ReadOrderDTO>> GetCustomerOrdersAsync(string CustomerID);
        public Task<ICollection<ReadOrderDTO>> GetAllOrdersAsync();
        public Task<ICollection<ReadOrderDTO>> GetOrderByIdAsync(int OrderID);
    }
}
