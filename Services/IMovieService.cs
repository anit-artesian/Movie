using MediaApplication.Models;
using System.Collections.Generic;

namespace MediaApplication.Services
{

    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        void AddMovie(Movie movie);

    }
}