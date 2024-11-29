using System;
using Backend.DTOs;

namespace Backend.Services.AccountService;

public interface IAccountService
{
    public Task<GetUserLoginDTO?> LoginAsync(LoginDTO loginUser);
    public Task<GetUserLoginDTO?> RegisterAsync(RegisterDTO registerUser);
}
