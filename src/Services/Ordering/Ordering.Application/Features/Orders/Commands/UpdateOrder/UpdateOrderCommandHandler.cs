using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IMapper mapper, ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
           var orderToUpdate= await _orderRepository.GetByIdAsync(request.Id);
            if(orderToUpdate== null)
            {
                throw new NotFoundException(nameof(Order),request.Id);
               
            }

            _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await _orderRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
            return Unit.Value;
        }
    }
}
