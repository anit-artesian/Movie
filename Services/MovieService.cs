using MediaApplication.Models;
using System.Collections.Generic;
using MediaApplication.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MediaApplication.Services
{

    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        public MovieService(ApplicationDbContext context)
        {
            this._context = context;


        }
        public List<Movie> GetAllMovies()
        {
            List<Movie> allMovies = new List<Movie>();
            allMovies = _context.Movies
                            .Include(c => c.Directors)
                            .Include(c => c.Writers)
                            .Include(c => c.Stars)
                            .Include(c => c.Images)
                            .Include(c => c.Genre)
                            .OrderByDescending(x => x.Id).ToList();
            return allMovies;

        }
        public void AddMovie(Movie movie)
        {

            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

    }
}

