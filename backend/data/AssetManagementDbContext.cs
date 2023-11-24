using asset_management_system_orm.Models;
using Microsoft.EntityFrameworkCore;

namespace asset_management_system_orm.data
{
    public class AssetManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }

        public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options) : base(options)
        {
        }

        public List<User> GetAllUsers()
        {
            return Users.ToList();
        }

        public void AddUser(User user)
        {
            Users.Add(user);
            SaveChanges();
        }

        public User? GetUserById(int id)
        {
            return Users.Find(id);
        }

        public User? GetUserByCredentials(string email, string password)
        {
            return Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }

    public class Asset
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public decimal Cost { get; set; }
        public string ResponsiblePerson { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
