using AutoMapper;
using DeliveryApplication.Dtos;
using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost("newOrder")]
        public async Task<IActionResult> CreateNewOrder(OrderDto newOrder)
        {
            if (NewOrderFieldsValidation(newOrder, out string message))
                return BadRequest(message);

            Order createdOrder = uow.OrderRepository.CreateNewOrder(mapper.Map<Order>(newOrder));
            await uow.SaveAsync();
            return Ok(createdOrder.Id);
        }

        private bool NewOrderFieldsValidation(OrderDto newOrder, out string message)
        {

            if (newOrder.DeliveryAddress.Length == 0)
            {
                message = "Address field must be filled.";
                return true;
            }

            message = "";
            return false;
        }

        [HttpGet("pendingOrders")]
        public async Task<IActionResult> GetPendingOrders()
        {
            try
            {
                var pendingOrders = await uow.OrderRepository.GetPendingOrders();
                var pendingOrdersDto = mapper.Map<List<OrderDto>>(pendingOrders);
                return Ok(pendingOrdersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("finishedOrders")]
        public async Task<IActionResult> GetFinishedOrders()
        {
            try
            {
                var finishedOrders = await uow.OrderRepository.GetFinishedOrders();
                var finishedOrdersDto = mapper.Map<List<OrderDto>>(finishedOrders);
                return Ok(finishedOrdersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("finishedOrders/{delivererId}")]
        public async Task<IActionResult> GetMyFinishedOrders(int delivererId)
        {
            try
            {
                var user = await uow.UserRepository.FindUser(delivererId);
                var finishedOrders = await uow.OrderRepository.GetMyFinishedOrders(user);
                var finishedOrdersDto = mapper.Map<List<OrderDto>>(finishedOrders);
                return Ok(finishedOrdersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("startedOrders")]
        public async Task<IActionResult> GetStartedOrders()
        {
            try
            {
                var startedOrders = await uow.OrderRepository.GetStartedOrders();
                var startedOrdersDto = mapper.Map<List<OrderDto>>(startedOrders);
                return Ok(startedOrdersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("approveOrder/{id}/{delivererId}")]
        public async Task<IActionResult> ApproveOrder(int id, int delivererId)
        {
            try
            {
                Order order = await uow.OrderRepository.GetOrderById(id);
                order.OrderState = OrderStateEnum.ACTIVE;
                order.Deliverer = await uow.UserRepository.FindUser(delivererId);
                await uow.SaveAsync();
                OrderDto orderDto = mapper.Map<OrderDto>(order);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("finishOrder/{id}/{delivererId}")]
        public async Task<IActionResult> FinishOrder(int id, int delivererId)
        {
            try
            {
                Order order = await uow.OrderRepository.GetOrderById(id);
                order.OrderState = OrderStateEnum.FINISHED;
                order.Deliverer = await uow.UserRepository.FindUser(delivererId);
                await uow.SaveAsync();
                OrderDto orderDto = mapper.Map<OrderDto>(order);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var allOrders = await uow.OrderRepository.GetAllOrders();
                var allOrdersDto = mapper.Map<List<OrderDto>>(allOrders);
                return Ok(allOrdersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getMyPendingOrders/{userId}")]
        public async Task<IActionResult> GetMyPendingOrders(int userId)
        {
            var userOrders = await uow.UserOrderRepository.GetMyOrders(userId);

            List<int> orderIds = new List<int>();
            foreach (var item in userOrders)
                orderIds.Add(item.OrderId);

            Order pendingOrder = new Order();
            foreach (var item in orderIds)
            {
                try
                {
                    pendingOrder = await uow.OrderRepository.DoesOrderPending(item);
                }
                catch (Exception)
                {
                    continue;
                }
            }
                

            if (pendingOrder.DeliveryAddress == null)
                return BadRequest("You don't have order in progress");
            return Ok(mapper.Map<OrderDto>(pendingOrder));
        }

        [HttpGet("getUserFinishedOrders/{userId}")]
        public async Task<IActionResult> GetUserFinishedOrders(int userId)
        {
            var userOrders = await uow.UserOrderRepository.GetMyOrders(userId);

            List<int> orderIds = new List<int>();
            foreach (var item in userOrders)
                orderIds.Add(item.OrderId);

            var finishedOrders = new List<Order>();
            foreach (var item in orderIds)
                finishedOrders.Add(await uow.OrderRepository.GetUserFinishedOrder(item));

            if (finishedOrders.Count() == 0)
                return BadRequest("You don't have finished orders");
            return Ok(mapper.Map<List<OrderDto>>(finishedOrders));
        }

        [HttpGet("getMyActiveOrder/{delivererId}")] 
        public async Task<IActionResult> GetMyActiveOrder(int delivererId)
        {
            try
            {
                var activeOrder = await uow.OrderRepository.GetMyActiveOrder(delivererId);
                return Ok(activeOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
