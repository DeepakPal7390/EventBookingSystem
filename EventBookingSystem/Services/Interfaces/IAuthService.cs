using EventBookingSystem.Contracts.Requests;
using EventBookingSystem.Contracts.Responses;

namespace EventBookingSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
