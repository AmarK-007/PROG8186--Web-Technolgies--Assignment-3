using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Shoecart_ASP_Assignment3.Data;
using Shoecart_ASP_Assignment3.Models;
using System.Threading.Tasks;

namespace Shoecart_ASP_Assignment3.EndPoints {
    public static class UserEndpoints {

        public static async Task<IActionResult> CreateUser(User newUser, MyDBContext db) {
            db.User.Add(newUser);
            await db.SaveChangesAsync();

            var location = $"/api/User/{newUser.user_id}";
            var responseContent = new {
                message = "User created",
                user = newUser
            };

            return new CreatedResult(location, responseContent);
        }

        public static async Task<IActionResult> UpdateUser(string username, User updateUser, MyDBContext db) {
            var user = await db.User.FirstOrDefaultAsync(u => u.username == username);
            if (user == null) {
                return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            user.name = updateUser.name;
            user.email = updateUser.email;
            user.password = updateUser.password;
            user.username = updateUser.username;
            user.purchase_history = updateUser.purchase_history;
            user.shipping_address = updateUser.shipping_address;

            await db.SaveChangesAsync();

            return new JsonResult(new { message = "User updated" });
        }

        public static async Task<IActionResult> DeleteUser(string username, MyDBContext db) {
            var user = await db.User.FirstOrDefaultAsync(u => u.username == username);
            if (user == null) {
                return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return new JsonResult(new { message = "User deleted" });
        }
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes) {
            var group = routes.MapGroup("/api/User").WithTags(nameof(User));

            group.MapGet("/", async (MyDBContext db) => {
                var users = await db.User.ToListAsync();
                return new JsonResult(users);
            })
            .WithName("GetAllUsers")
            .Produces<User[]>(StatusCodes.Status200OK);

            group.MapGet("/{username:alpha}", async (string username, MyDBContext db) => {
                var user = await db.User.FirstOrDefaultAsync(u => u.username == username);
                if (user != null) {
                    return new JsonResult(user);
                } else {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("GetUserByUsername")
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{username:alpha}", async (string username, User updateUser, MyDBContext db) => {
                var user = await db.User.FirstOrDefaultAsync(u => u.username == username);
                if (user == null) {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }

                user.name = updateUser.name;
                user.email = updateUser.email;
                user.password = updateUser.password;
                user.username = updateUser.username;
                user.purchase_history = updateUser.purchase_history;
                user.shipping_address = updateUser.shipping_address;

                await db.SaveChangesAsync();

                return new JsonResult(new { message = "User updated" });
            })
            .WithName("UpdateUser")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (User newUser, MyDBContext db) => {
                db.User.Add(newUser);
                await db.SaveChangesAsync();

                return new CreatedResult($"/api/User/{newUser.user_id}", new { message = "User created", user = newUser });
            })
            .WithName("CreateUser")
            .Produces<User>(StatusCodes.Status201Created);

            group.MapDelete("/{username:alpha}", async (string username, MyDBContext db) => {
                var user = await db.User.FirstOrDefaultAsync(u => u.username == username);
                if (user == null) {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }

                db.User.Remove(user);
                await db.SaveChangesAsync();

                return new JsonResult(new { message = "User deleted" });
            })
            .WithName("DeleteUser")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
