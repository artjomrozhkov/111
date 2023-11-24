using asset_management_system_orm.data;
using asset_management_system_orm.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace asset_management_system_orm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class AssetManagementController : ControllerBase
    {
        private readonly AssetManagementDbContext _context;

        public AssetManagementController(AssetManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("assets")]
        public ActionResult<IEnumerable<Asset>> GetAssets()
        {
            var assets = _context.Assets.ToList();
            return Ok(assets);
        }

        [HttpGet("user-assets")]
        public ActionResult<IEnumerable<Asset>> GetUserAssets()
        {
            var assets = _context.Assets.ToList();
            return Ok(assets);
        }

        [HttpGet("assets/{id}")]
        public ActionResult<Asset> GetAsset(int id)
        {
            var asset = _context.Assets.Find(id);

            if (asset == null)
            {
                return NotFound(new { error = "Asset not found" });
            }

            return Ok(asset);
        }

        [HttpPost("assets")]
        public ActionResult<Asset> CreateAsset([FromBody] Asset newAsset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "One or more parameters are missing" });
            }

            _context.Assets.Add(newAsset);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAsset), new { id = newAsset.Id }, newAsset);
        }

        [HttpPut("assets/{id}")]
        public ActionResult<Asset> UpdateAsset(int id, [FromBody] Asset updatedAsset)
        {
            var asset = _context.Assets.Find(id);

            if (asset == null)
            {
                return NotFound(new { error = "Asset not found" });
            }

            asset.Number = updatedAsset.Number;
            asset.Name = updatedAsset.Name;
            asset.State = updatedAsset.State;
            asset.Cost = updatedAsset.Cost;
            asset.ResponsiblePerson = updatedAsset.ResponsiblePerson;
            asset.AdditionalInformation = updatedAsset.AdditionalInformation;

            _context.SaveChanges();

            return Ok(updatedAsset);
        }

        [HttpDelete("assets/{id}")]
        public IActionResult DeleteAsset(int id)
        {
            var asset = _context.Assets.Find(id);

            if (asset == null)
            {
                return NotFound(new { error = "Asset not found" });
            }

            _context.Assets.Remove(asset);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("login/{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { error = "Both email and password are required" });
            }

            var user = _context.GetUserByCredentials(email, password);

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            var userResponse = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                isAdmin = user.IsAdmin
            };

            return Ok(new { message = "Login successful", user = userResponse });
        }

        [HttpPost("register/{name}/{email}/{password}")]
        public IActionResult Register(string name, string email, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { error = "Name, email, and password are required" });
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
            {
                return Conflict(new { error = "This email has already been taken" });
            }

            var newUser = new User(name, email, password, isAdmin: false);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Login successful" });
        }
        [HttpGet("users")]
        public List<User> Get()
        {
            return _context.Users.ToList(); ;
        }
    }
}
