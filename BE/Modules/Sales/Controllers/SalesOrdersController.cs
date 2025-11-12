using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using ERP_API.DTOS.Order;
using ERP_API.Core.Database.Entities;
using ERP_API.Repositories;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _service;
        private readonly ICustomerService _customerService;
        private readonly ISaleStaffService _saleStaffService;

        public SalesOrdersController(ISalesOrderService service, ICustomerService customerService, ISaleStaffService saleStaffService)
        {
            _service = service;
            _customerService = customerService;
            _saleStaffService = saleStaffService;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _service.GetAllOrdersAsync();

            var ordersDto = orders.Select(o => Mappers.SalesOrderMapper.ToDto(o)).ToList();

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

            var orderDto = Mappers.SalesOrderMapper.ToDto(order);

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateSalesOrderDTO createOrderDTO)
        {
            var orderEntity = Mappers.SalesOrderMapper.ToCreateEntity(createOrderDTO);

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var saleStaff = await _saleStaffService.GetSaleStaffByIdAsync(userId);
            orderEntity.Staff = saleStaff;

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