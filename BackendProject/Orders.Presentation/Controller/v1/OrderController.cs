using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Application.Interfaces;
using Orders.Domain.Models;
using Asp.Versioning;

namespace Orders.Presentation.Controller.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/order")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", id);
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order model received.");
                return BadRequest(ModelState);
            }

            await _orderService.AddOrderAsync(order);
            _logger.LogInformation("Order added successfully.");
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                _logger.LogWarning("Order ID mismatch: route ID = {RouteId}, body ID = {BodyId}", id, order.Id);
                return BadRequest("Order ID mismatch.");
            }

            await _orderService.UpdateOrderAsync(order);
            _logger.LogInformation("Order with ID {OrderId} updated.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            await _orderService.DeleteOrderAsync(id);
            _logger.LogInformation("Order with ID {OrderId} deleted.", id);
            return NoContent();
        }
    }

}

