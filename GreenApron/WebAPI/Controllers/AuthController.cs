using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private GreenApronContext _context { get; set; }

        public AuthController(GreenApronContext context)
        {
            _context = context;
        }

        // POST api/auth/login
        [HttpPost]
        public async Task<JsonResult> Login([FromBody] User user)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");

            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return Json(new AuthResponse{ success = false, message = "Something went wrong, please resubmit with all required fields." });
            }
            // Check for existing username in database
            User userCheck = await _context.User.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (userCheck != null)
            {
                if (userCheck.Password == user.Password)
                {
                    return Json(new AuthResponse{ success = true, message = "You have successfully logged in.", user = userCheck });
                } else
                {
                    return Json(new AuthResponse{ success = false, message = "Wrong password, please try again" });
                }
            } else
            {
                return Json(new AuthResponse{ success = false, message = "I couldn't find a user with that username, please try again." });
            }
        }

        // POST api/auth/register
        [HttpPost]
        public async Task<JsonResult> Register([FromBody] User user)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return Json(new AuthResponse{ success = false, message = "Something went wrong, please resubmit with all required fields." });
            }
            // Check for existing username in database
            User usernameCheck = await _context.User.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (usernameCheck == null)
            {
                // Register the new user
                _context.User.Add(user);
                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new AuthResponse{ success = true, message = "You have successfully registered as a new user.", user = user });
                }
                catch
                {
                    return Json(new AuthResponse{ success = false, message = "Something went wrong while saving to the database, please try again." });
                }
            } else
            {
                return Json(new AuthResponse{ success = false, message = "A user already exists with this username, please try again." });
            }
        }

        // DELETE api/auth/delete/username
        [HttpDelete("{username}")]
        public async Task<JsonResult> Delete(string username)
        {
            User user = await _context.User.SingleOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                _context.User.Remove(user);
                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new AuthResponse{ success = true, message = "User successfully deleted" });
                }
                catch
                {
                    return Json(new AuthResponse{ success = false, message = "Database updates failed, please try again." });
                }
            } else
            {
                return Json(new AuthResponse{ success = false, message = "User not found." });
            }
        }
    }
}
