using AutoMapper;
using MediaApplication.Models.MovieViewModels;

namespace MediaApplication.Models
{

    
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<AddMovieViewModel, Movie>();
            CreateMap<Movie, MovieViewModel>().MaxDepth(1);               
            CreateMap<Star, StarViewModel>().MaxDepth(1);               
            CreateMap<Genre, GenreViewModel>().MaxDepth(1);               
            CreateMap<Director, DirectorViewModel>().MaxDepth(1);               
            CreateMap<Writer, WriterViewModel>().MaxDepth(1);               
            CreateMap<Image, ImageViewModel>().MaxDepth(1);               
           
        }
    }
}