using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using BlazorPracticeApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.Service
{
    public class UserService: IUserService
    {
        public readonly ContextDb context;
        private readonly JwtGenerator jwtGenerator;
        private readonly IHttpContextAccessor accessor;

        public UserService(ContextDb _context, JwtGenerator _jwtGenerator, IHttpContextAccessor _accessor)
        {
            context = _context;
            jwtGenerator = _jwtGenerator;
            accessor = _accessor;
        }

            public async Task<IActionResult> GetAllUsers()
            {
                var users = context.Users.Where(p => p.Email != "ilnaz@gmail.com").ToListAsync();
                return new OkObjectResult(new
                {
                    status = true,
                    list = users
                });
            }

        public async Task<IActionResult> UpdateUser(int id, NewUserDto update_user)
        {
            bool IsEmail = context.Users.Any(p => p.Email == update_user.Email && p.Id == id);
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user != null && IsEmail)
            {
                user.Email = update_user.Email;
                user.Password = update_user.Password;
                user.Name = update_user.Name;
                user.Description = update_user.Description;
                user.RoleId = update_user.RoleId;
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Пользователь успешно обновлен"
                });
            }
            else if (user == null && !IsEmail)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Пользователь с id {id} не существует и указанный вами Email занят"
                }
                );
            }
            else if (!IsEmail)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Указанный вами Email занят"
                }
                );
            }

            else if (user == null)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Пользователь с id  {id} не существует"
                }
                );
            }

            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Ошибка"
                }
                );
            }
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Пользователь успешно удален"
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Пользователь с id {id} не существует"
                }
                );
            }
        }

        public async Task<IActionResult> CreateUser(NewUserDto newuser)
        {
            var email = await context.Users.FirstOrDefaultAsync(x => x.Email == newuser.Email.ToLower());

            if (email == null)
            {
                var user = new User()
                {
                    Email = newuser.Email.ToLower(),
                    Password = newuser.Password,
                    Name = newuser.Name,
                    Description = newuser.Description,
                    RoleId = newuser.RoleId,
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Пользователь добавлен"
                }
                );
            }
            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Указанный вами email занят"
                }
                );
            }
        }

        public async Task<IActionResult> RegistrationUser(NewUserDto newuser)
        {
            var email = await context.Users.FirstOrDefaultAsync(x => x.Email == newuser.Email.ToLower());

            if (email == null)
            {
                var user = new User()
                {
                    Email = newuser.Email.ToLower(),
                    Password = newuser.Password,
                    Name = newuser.Name,
                    Description = newuser.Description,
                    RoleId = 2
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Вы успешно зарегистрировались"
                }
                );
            }

            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Указанный вами Email занят"
                }
                );
            }


        }

        public async Task<IActionResult> AuthUser(AuthDto user)
        {
            var auth_user = await context.Users.FirstOrDefaultAsync(p => p.Email == user.email.ToLower() && p.Password == user.password);

            if (auth_user == null)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Такого пользователя не существует"
                }
                );
            }

            string token = jwtGenerator.GenerateJwt(auth_user.Id, auth_user.RoleId);
            var session = new Session()
            {
                Token = token,
                UserId = auth_user.Id,
            };
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                token = token,
                roleId = auth_user.RoleId,
                name = auth_user.Name,
            });
        }
    }
}
