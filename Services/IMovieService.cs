using MediaApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaApplication.Services
{

    public interface IMovieService
    {
        List<Movie> GetAllMovies();
         Task<List<Movie>> GetAllMoviess();
        void AddMovie(Movie movie);
        Movie GetMovie();
        Movie GetMovieFirst();

    }
}