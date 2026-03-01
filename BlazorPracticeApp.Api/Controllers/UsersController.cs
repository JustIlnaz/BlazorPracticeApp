using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using BlazorPracticeApp.ApiRequest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPracticeApp.Api.Controllers
{
    public class UsersController 
    {
        public readonly IUserService service;
        public UsersController(IUserService _service)
        {
            service = _service;
        }

        [HttpGet]
        [Route("/api/GetAll")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> GetAll()
        {
            return await service.GetAllUsers();
        }

        [HttpPost]
        [Route("/api/CreateUser")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> CreateUser([FromBody] NewUserDto newuser)
        {
            return await service.CreateUser(newuser);
        }

        [HttpPut]
        [Route("/api/UpdateUser/{id}")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] NewUserDto newuser)
        {
            return await service.UpdateUser(id, newuser);
        }

        [HttpDelete]
        [Route("/api/DeleteUser/{id}")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await service.DeleteUser(id);
        }

        [HttpPost]
        [Route("/api/AuthUser")]
        public async Task<IActionResult> AuthUser([FromBody] AuthDto user)
        {
            return await service.AuthUser(user);
        }

        [HttpPost]
        [Route("/api/RegUser")]
        public async Task<IActionResult> RegUser([FromBody] NewUserDto newuser)
        {
            return await service.RegistrationUser(newuser);
        }
    }
}
