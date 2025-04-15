using EventBookingSystem.Models.Domain;

namespace EventBookingSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
    }
}
