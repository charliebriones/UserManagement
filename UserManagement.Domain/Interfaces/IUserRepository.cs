using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);       // use int
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);              // delete by id
}
