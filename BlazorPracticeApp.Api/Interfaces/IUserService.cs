using BlazorPracticeApp.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPracticeApp.Api.Interfaces
{
    public interface IUserService
    {

        Task<IActionResult> GetAllUsers();
        Task<IActionResult> UpdateUser(int id, NewUserDto update_user);
        Task<IActionResult> DeleteUser(int id);
        Task<IActionResult> CreateUser(NewUserDto newuser);
        Task<IActionResult> RegistrationUser(NewUserDto newuser);
        Task<IActionResult> AuthUser(AuthDto user);

    }
}
