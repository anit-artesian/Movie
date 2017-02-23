using AutoMapper;
using MediaApplication.Models.MovieViewModels;

namespace MediaApplication.Models
{

    
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<AddMovieViewModel, Movie>();
            CreateMap<Movie, AllMovieViewModel>();
           
        }
    }
}