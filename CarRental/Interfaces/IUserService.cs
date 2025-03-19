using CarRental.Models;
using CarRental.Models.DTOs.Token;
using CarRental.Models.DTOs.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CarRental.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResponse<bool>> RegisterAsync(UserRegisterDTO dto);
        public Task<ServiceResponse<TokenDTO>> LoginAsync(UserLoginDTO dto);
        public Task<ServiceResponse<bool>> VerifyAccountAsync(string telNum, string code);
        public Task<ServiceResponse<bool>> ResendVerificationCode(int userId);
    }
}
