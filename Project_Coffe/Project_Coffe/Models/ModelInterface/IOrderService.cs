using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersByUserId(int userId);
        Task<Order> GetOrderById(int id);
        Task<Order> CreateOrder(Order order, User user);
        Task<bool> DeleteOrder(int id);
    }
}
