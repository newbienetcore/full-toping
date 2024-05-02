using Microsoft.EntityFrameworkCore;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.UseCases.Order.Crud
{
    public class CrudOrderFlow
    {
        public readonly DateTime currentTime = DateTime.Now;
        private readonly IDataContext _dbContext;
        public CrudOrderFlow(IDataContext ctx)
        {
            _dbContext = ctx;
        }
        public async Task<Response> CreateOrderAndProductOrders(OrderSchema order, List<ProductOrderSchema> productOrders)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    order.CreatedAt = currentTime;
                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();
                    foreach (var productOrder in productOrders)
                    {
                        productOrder.OrderID = order.Id;
                        _dbContext.ProductOrders.Add(productOrder);
                        _dbContext.SaveChanges();
                    }
                    transaction.Commit();
                    return new Response(Message.SUCCESS, order);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response(Message.ERROR, ex.Message);
                }
            }
        }
    }
}
