#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;

namespace Petdora.Models;
public class DataContext : DbContext 
{   
    public DataContext(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<UserPetFollow> UserPetFollows { get; set; }
    public DbSet<UserPostLike> UserPostLikes { get; set; }
}