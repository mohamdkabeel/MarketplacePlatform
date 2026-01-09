using Marketplace.Application.Dtos.Orders;
using Marketplace.Application.IServices.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.Order
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> Pay(int orderId)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            // هنا ممكن تضيف أي Call لـ Stripe أو PayPal API
            var order = await _orderService.GetOrderByIdAsync(orderId, userId);
            if (order == null) return NotFound();

            // Simulate payment success
            await _orderService.UpdateOrderStatusAsync(orderId, "Paid");

            return Ok(new { message = "Payment successful", order });
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CreateOrderDto dto)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var order = await _orderService.CreateOrderAsync(userId, dto);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var order = await _orderService.GetOrderByIdAsync(id, userId);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var success = await _orderService.UpdateOrderStatusAsync(id, status);
            return success ? Ok("Updated") : NotFound();
        }
    }

}
