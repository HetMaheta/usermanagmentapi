using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Data;
using System.Text.RegularExpressions;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // GET all users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                return Ok(UserStore.Users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        // GET user by ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var user = UserStore.Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    return NotFound($"User with ID {id} not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST create user
        [HttpPost]
        public ActionResult<User> CreateUser(User newUser)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(newUser.Name))
                    return BadRequest("Name is required");

                if (!IsValidEmail(newUser.Email))
                    return BadRequest("Invalid email format");

                // Safe ID generation
                int newId = UserStore.Users.Any() ? UserStore.Users.Max(u => u.Id) + 1 : 1;
                newUser.Id = newId;

                UserStore.Users.Add(newUser);

                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            try
            {
                var user = UserStore.Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    return NotFound($"User with ID {id} not found");

                if (string.IsNullOrWhiteSpace(updatedUser.Name))
                    return BadRequest("Name is required");

                if (!IsValidEmail(updatedUser.Email))
                    return BadRequest("Invalid email format");

                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = UserStore.Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    return NotFound($"User with ID {id} not found");

                UserStore.Users.Remove(user);

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Email validation helper
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}