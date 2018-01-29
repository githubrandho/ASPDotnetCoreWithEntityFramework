using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;
using DutchTreat.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var userName = User.Identity.Name;
                var results = _repository.GetAllOrdersByUser(userName);
                return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get orders");
                return BadRequest("Failed to get orders");
            }
        }


        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null)
                {
                    return Ok(_mapper.Map<Order,OrderViewModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get order");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var newOrder = new Order()
                    //{
                    //    OrderDate = model.OrderDate,
                    //    OrderNumber = model.OrderNumber,
                    //    Id = model.OrderId

                    //};
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);
                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                        //var vm = new OrderViewModel()
                        //{
                        //    OrderId = newOrder.Id,
                        //    OrderDate = newOrder.OrderDate,
                        //    OrderNumber = newOrder.OrderNumber
                        //};
                        var vm = _mapper.Map<Order, OrderViewModel>(newOrder);
                        return Created($"/api/order/{vm.OrderId}", vm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new order");
            }
            return BadRequest("Failed to save new order");
        }
    }
}