using EventBookingSystem.Data;
using EventBookingSystem.Models.Domain;
using EventBookingSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly EventDbContext _dbContext;

    public UserRepository(EventDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}

