using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Orders;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using System.Collections.Generic;
using OnlineStore.Api.Infrastructure.Specifications;
using System;
using OnlineStore.Api.Application.Orders.Interfaces;
using System.Linq;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;

namespace OnlineStore.Api.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly ICrudService<Order, OrderDto> _ordersCrudService;
        private readonly IOrderQueueService _orderQueueService;

        private readonly IRepository<OrderShipment> _orderShipmentRepository;
        public OrdersController(ICrudService<Order, OrderDto> ordersCrudService,
            IOrderQueueService orderQueueService,

            IRepository<OrderShipment> orderShipmentRepository
            )
        {
            _ordersCrudService = ordersCrudService;
            _orderQueueService = orderQueueService;
            _orderShipmentRepository = orderShipmentRepository;
        }


        /// <summary>
        /// Creates new order and selects products
        /// </summary>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody] OrderDto dto, CancellationToken cancellationToken)
        {
            if (!dto.OrderProducts.Any())
            {
                return BadRequest(new ApiError { ErrorCode = ErrorCode.HttpStatus400.RequiredValue, ErrorMessage = ErrorCode.HttpStatus400.RequiredValueMessage });
            }

            await _orderQueueService.CreateAsync(dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [HttpGet()]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var order = await _ordersCrudService.GetAsync(new AllNotDeleted<Order>(), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Get order by its id
        /// </summary>
        /// <param name="id">id of the order</param>
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await _ordersCrudService.GetByIdAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Updates a order by its id
        /// </summary>
        /// <param name="id">id of the order</param>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] OrderDto dto, CancellationToken cancellationToken)
        {
            var order = await _ordersCrudService.GetByIdAsync(id, cancellationToken);
            order.Status = dto.Status;
            order.ShippingDate = dto.ShippingDate;
            if (dto.ShippingDate.HasValue && order.ShippingAddress == null)
            {
                await _orderShipmentRepository.SaveAsync(new OrderShipment { OrderDate = dto.Created, ShippingDate = dto.ShippingDate.Value }, cancellationToken);
            }

            await _ordersCrudService.UpdateAsync(id, order, cancellationToken);
            return NoContent();
        }
    }
}
