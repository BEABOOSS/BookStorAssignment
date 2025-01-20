using BookStore.Models;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class BookStoreContext : IdentityDbContext<UserIdentity>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<BookStore.Models.Book> Book { get; set; } = default!;
        public DbSet<BookStore.Models.UserIdentity> UserIdentity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource = identityDb; Cache=Shared");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }




        public bool DoesBookExist(int id)
        {
            // Remove://
            Console.WriteLine("Service Does book exist");
            return this.Book.Any(e => e.Id == id);
        }
    }
}
