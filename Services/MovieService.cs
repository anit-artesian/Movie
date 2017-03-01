using MediaApplication.Models;
using System.Collections.Generic;
using MediaApplication.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using MediaApplication.Models.MovieViewModels;

namespace MediaApplication.Services
{

    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        public MovieService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public List<Movie> GetAllMovies(int pageSize,int pageNumber)
        {
            int recordsToSkip =  (pageNumber * pageSize) - pageSize;
            int recordsToTake =  pageSize;
            List<Movie> allMovies = new List<Movie>();
            allMovies = _context.Movies
                            .Include(c => c.Directors)
                            .Include(c => c.Writers)
                            .Include(c => c.Stars)
                            .Include(c => c.Images)
                            .Include(c => c.Genre)
                            .OrderByDescending(x => x.Id).Skip(recordsToSkip).Take(recordsToTake).ToList();
            return allMovies;
        }
        public async Task<List<Movie>> GetAllMoviess()
        {
            List<Movie> allMovies = new List<Movie>();

            allMovies = await Task.Run(() => _context.Movies
                            .Include(c => c.Directors)
                            .Include(c => c.Writers)
                            .Include(c => c.Stars)
                            .Include(c => c.Images)
                            .Include(c => c.Genre)
                            .OrderBy(x => x.Id).ToList());
            return allMovies;
        }
        public  List<GenreViewModel> GetGenre(){

              List<GenreViewModel> genreList = Enum.GetValues(typeof(GenreEnum)).Cast<GenreEnum>().Select(g => new GenreViewModel
                {
                    Id = (int)g,
                    Title = g.ToString()
                }).ToList();
                return genreList;
        }

        public   Movie GetMovieDetails(int id)
        {
            return _context.Movies .Include(c => c.Directors)
                            .Include(c => c.Writers)
                            .Include(c => c.Stars)
                            .Include(c => c.Images)
                            .Include(c => c.Genre).FirstOrDefault(m=>m.Id == id);

        }
        public void AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public Movie GetMovie()
        {
            var test = _context.Movies.Where(x => x.Title == "title");
            return test.FirstOrDefault();
        }
        public Movie GetMovieFirst()
        {
            return _context.Movies.FirstOrDefault(x => x.Title == "title");
        }

        

    }
}

