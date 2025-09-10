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
            bool isValidIds = await _orderService.HasOrderValidProductIds(order);
            if (!isValidIds)
            {
                _logger.LogWarning("Product ids mismatch. One or more product IDs are invalid.");
                return BadRequest("Product ids mismatch. One or more product IDs are invalid.");
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

            bool isValidIds = await _orderService.HasOrderValidProductIds(order);
            if (!isValidIds)
            {
                _logger.LogWarning("Product ids mismatch.");
                return BadRequest("Product ids mismatch.");
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

        [HttpPut("{id}/close")]
        public async Task<IActionResult> CloseOrder(long id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID");
                return BadRequest("Invalid ID");
            }

            await _orderService.CloseOrderAsync(id);
            return NoContent();
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            try
            {
                var orders = await _orderService.GetPendingOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving pending orders.", details = ex.Message });
            }
        }
    }

}

