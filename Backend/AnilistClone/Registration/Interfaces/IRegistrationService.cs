using AnilistClone.Registration.DTOs.Requests;
using AnilistClone.Registration.DTOs.Responses;

namespace AnilistClone.Registration.Interfaces
{
    public interface IRegistrationService
    {
        public Task<RegistrationResponse> RegisterUser(RegistrationRequest request);
    }
}
