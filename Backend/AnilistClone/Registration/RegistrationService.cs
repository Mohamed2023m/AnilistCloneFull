using AnilistClone.AnilistClone.Data;
using AnilistClone.Exceptions;
using AnilistClone.Models;
using AnilistClone.Models.Enums;
using AnilistClone.Registration.DTOs.Requests;
using AnilistClone.Registration.DTOs.Responses;
using AnilistClone.Registration.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace AnilistClone.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;

        public RegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationResponse> RegisterUser(RegistrationRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == request.Username
            );

            if (existingUser != null)
            {
                throw new UserExist();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var registeredUser = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                UserType = UserType.User,
            };

            _context.Users.Add(registeredUser);
            await _context.SaveChangesAsync();

            var successResponse = new RegistrationResponse
            {
                Username = registeredUser.Username,
                Message = "Account Created",
            };

            return successResponse;
        }
    }
}
