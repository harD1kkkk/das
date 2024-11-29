using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models;
using Project_Coffe.Models.ModelRealization;
using System.Security.Claims;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetAllOrdersByUserId(userId);
            return Ok(orders);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Order order = new Order
            {
                UserId = userId,
                OrderProducts = model.Products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            await _orderService.CreateOrder(order);
            return Ok(order);
        }

    }
}
