
using System.Linq;
using MediaApplication.Data;
namespace MediaApplication.Models
{

    public static class DbInitializer
    {

        public static void Initialize(ApplicationDbContext context)
        {

            context.Database.EnsureCreated();

           // Look for any Genres.
            if (context.Genres.Any())
            {
                return;   // DB has been seeded
            }
            
            var genres =  new Genre[]
            {
                new Genre{Title="Comedy"},
                new Genre{Title="Action"},
                new Genre{Title="Romance"},
                new Genre{Title="War"}

            };

            context.Genres.AddRange(genres);
            context.SaveChanges();
        }
    }
}

