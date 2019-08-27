using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoPets.Domain.DataTransferObjects;
using ContosoPets.Domain.Models;
using ContosoPets.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        // GET api/orders    
        [HttpGet]
        public async Task<List<CustomerOrder>> Get()
        {
            return await orderService.GetAll();
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public async Task<CustomerOrder> Get(int id)
        {
            return await orderService.GetById(id);
        }

        // POST api/orders
        [HttpPost]
        public async Task<Order> Post([FromBody] Order newOrder)
        {
            return await orderService.Create(newOrder);
        }

        // PUT api/orders/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id)
        {
            return await orderService.SetFulfilled(id);
        }

        // DELETE api/orders/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await orderService.Delete(id);
        }
    }
}