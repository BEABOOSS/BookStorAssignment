using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public static class SeedData
    {
        public static void Initilize(IServiceProvider serviceProvider)
        {


            using (var context = new BookStoreContext(
                serviceProvider.GetRequiredService<DbContextOptions<BookStoreContext>>()))
            {
                if (context.Book.Any())
                {
                    return;
                }
                context.Book.AddRange(
                    new Book
                    {
                        Title = "aliquet magna a",
                        Author = "Damon Moran",
                        Price = 49.77M
                    },
                    new Book
                    {
                        Title = "varius orci, in",
                        Author = "Quinn Walton",
                        Price = 42.40M
                    },
                    new Book
                    {
                        Title = "malesuada vel, convallis",
                        Author = "Octavius Schultz",
                        Price = 29.94M
                    },
                    new Book
                    {
                        Title = "velit eget laoreet",
                        Author = "Adele Hartman",
                        Price = 17.12M
                    },
                    new Book
                    {
                        Title = "velit eget laoreet Part 2",
                        Author = "Adele Hartman",
                        Price = 27.12M
                    },
                    new Book
                    {
                        Title = "metus. Vivamus euismod",
                        Author = "Leroy Rhodes",
                        Price = 25.40M
                    });
                context.SaveChanges();
            }
        }
    }
}
