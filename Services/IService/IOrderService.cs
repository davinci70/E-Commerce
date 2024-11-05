using e_commerce.Helpers;
using e_commerce.Models.DTOs.OrderDTOs;

namespace e_commerce.Services.IService
{
    public interface IOrderService
    {
        public Task AddAsync(OrderDTO model);
        public Task UpdateAsync(int ID, UpdateOrderDTO model);
        public Task UpdateOrderStatusAsync(int OrderID, OrderStatus.enOrderStatus orderStatus);
        public Task<ICollection<OrderDTO>> GetCustomerOrdersAsync(string CustomerID);
    }
}
