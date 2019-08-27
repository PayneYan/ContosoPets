// Add the ContosoPets.Domain.Models using statement
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoPets.DataAccess.Data;
using ContosoPets.Domain.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ContosoPets.Domain.Models;
using System;
using ContosoPets.Interface;

namespace ContosoPets.DataAccess.Service
{
    public class OrderService : IOrderService
    {
        // Add the constructor code
        private readonly ContosoPetsContext context;
        public OrderService(ContosoPetsContext context)
        {
            this.context = context;
        }
        // Add the GetAll code
        public async Task<List<CustomerOrder>> GetAll()
        {
            //linq语句
            var orders = await (
                from o in context.Orders.AsNoTracking().TagWith(nameof(GetAll))
                orderby o.OrderPlaced descending
                select new CustomerOrder
                {
                    OrderId =o.Id,
                    CustomerName = $"{o.Customer.LastName},{o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue?
                    o.OrderFulfilled.Value.ToShortDateString():string.Empty,
                    OrderLineItems = (from po in o.ProductOrders
                                    select new OrderLineItem
                                    {
                                        ProductQuantity = po.Quantity,
                                        ProductName = po.Product.Name,
                                    })

                }
            ).ToListAsync();

            //linq方法
            var orders2 = context.Orders.AsNoTracking().OrderByDescending(m=>m.OrderPlaced).Select(o=> new CustomerOrder{
                OrderId = o.Id,
                CustomerName = $"{o.Customer.LastName},{o.Customer.FirstName}",
                OrderFulfilled = o.OrderFulfilled.HasValue ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                OrderLineItems = o.ProductOrders.Select(po=> new OrderLineItem{
                    ProductQuantity = po.Quantity,
                    ProductName = po.Product.Name
                })
            }).ToListAsync();

            return orders;
        }
        // Add the GetById code
        private IQueryable<Order> GetOrderById(int id) =>
            from o in context.Orders.AsNoTracking().TagWith(nameof(GetOrderById))
            where o.Id == id
            select o;

        private IQueryable<Order> GetOrderById2(int id) => context.Orders.AsTracking().Where(o=>o.Id == id);

        public async Task<CustomerOrder> GetById(int id)
        {
            var order = await (
                from o in GetOrderById(id)
                select new CustomerOrder
                {
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? 
                        o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (from po in o.ProductOrders
                                    select new OrderLineItem
                                    {
                                        ProductQuantity = po.Quantity,
                                        ProductName = po.Product.Name
                                    })
                }).TagWith(nameof(GetById))
                .FirstOrDefaultAsync();
            

            //linq方法
            var orders2 = context.Orders
                .AsNoTracking()
                .Where(o=>o.Id == id)
                .Select(o=> new CustomerOrder{
                    OrderId = o.Id,
                    CustomerName = $"{o.Customer.LastName},{o.Customer.FirstName}",
                    OrderFulfilled = o.OrderFulfilled.HasValue ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = o.ProductOrders
                                    .Select(po=> 
                                        new OrderLineItem {
                                            ProductQuantity = po.Quantity,
                                            ProductName = po.Product.Name
                                        })
            }).ToListAsync();
            return order;
        }
        // Add the Create code
        public async Task<Order> Create(Order newOrder)
        {
            newOrder.OrderPlaced = DateTime.UtcNow;
            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();

            return newOrder;
        }
        // Add the SetFulfilled code
        public async Task<bool> SetFulfilled(int id)
        {
            var isFulfilled = false;
            var order = await GetOrderById(id).FirstOrDefaultAsync();

            if (order != null)
            {
                order.OrderFulfilled = DateTime.UtcNow;
                context.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
                isFulfilled = true;
            }

            return isFulfilled;
        }
        // Add the Delete code
        public async Task<bool> Delete(int id)
        {
            var isDeleted = false;
            var order = await GetOrderById(id).FirstOrDefaultAsync();

            if (order != null)
            {
                context.Remove(order);
                await context.SaveChangesAsync();
                isDeleted = true;
            }
            return isDeleted;
        }

    }
}