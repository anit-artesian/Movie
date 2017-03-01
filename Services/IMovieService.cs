using MediaApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaApplication.Models.MovieViewModels;

namespace MediaApplication.Services
{

    public interface IMovieService
    {
        List<Movie> GetAllMovies(int pageSize,int pageNumber);
         Task<List<Movie>> GetAllMoviess();
        void AddMovie(Movie movie);
        Movie GetMovie();
        Movie GetMovieFirst();
        List<GenreViewModel> GetGenre();
        Movie GetMovieDetails(int id);

    }
}