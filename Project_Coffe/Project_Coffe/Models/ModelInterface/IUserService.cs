using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user);
        Task<User> LoginUser(string email, string password);
    }
}
