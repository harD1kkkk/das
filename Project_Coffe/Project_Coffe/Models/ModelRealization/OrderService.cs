using Microsoft.EntityFrameworkCore;
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Models.ModelRealization
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByUserId(int userId)
        {
            return await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }


        public async Task<Order> GetOrderById(int id)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id); 

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return order; 
        }


        public async Task<Order> CreateOrder(Order order, User user)
        {
            decimal totalAmount = 0;

            foreach (var orderProduct in user.Orders)
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == orderProduct.ProductId);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {orderProduct.Id} not found.");
                }

                if (product.Stock < orderProduct.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}.");
                }

                product.Stock -= orderProduct.Quantity;
                totalAmount += product.Price * orderProduct.Quantity;
            }

            order.TotalAmount = totalAmount;

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }


        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            foreach (var orderProduct in order.User.Orders)
            {
                var product = await _dbContext.Products.FindAsync(orderProduct.ProductId);
                if (product != null)
                {
                    product.Stock += orderProduct.Quantity;
                }
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
