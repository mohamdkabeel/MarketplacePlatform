
using Marketplace.Application.Dtos.Orders;


namespace Marketplace.Application.IServices.Order
{
   

    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto dto);
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string userId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }

}
