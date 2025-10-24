using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.DTOS.Order;
using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ICustomerService _customerService;

        public OrdersController(IOrderService service, ICustomerService customerService)
        {
            _service = service;
            _customerService = customerService;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _service.GetAllOrdersAsync();

            var ordersDto = orders.Select(o => Mappers.OrderMapper.ToDto(o)).ToList();

            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = Mappers.OrderMapper.ToDto(order);

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            // Solve more cases and validations as needed *

            var orderEntity = Mappers.OrderMapper.ToCreateEntity(createOrderDTO);

            // New customer validation
            if (createOrderDTO.CustomerId <= 0)
            {
                var customer = new Customer
                {
                    Name = createOrderDTO.Name,
                    Contact = createOrderDTO.Contact,
                    Address = createOrderDTO.Address
                };

                var isCreated = await _customerService.CreateCustomerAsync(customer);

                if (!isCreated)
                {
                    return BadRequest("Failed to create customer.");
                }
                orderEntity.CustomerId = customer.CustomerId;
            }

            var result = await _service.CreateOrderAsync(orderEntity);

            if (!result)
            {
                return BadRequest("Failed to create order.");
            }
            return CreatedAtAction(nameof(GetOrderById), new { id = orderEntity.SalesOrderId }, orderEntity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var result = await _service.DeleteOrderAsync(order);
            if (!result)
            {
                return BadRequest("Failed to delete order.");
            }

            return NoContent();
        }

    }
}