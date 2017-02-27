
using MediaApplication.Models.CommonMovieViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaApplication.Models.MovieAPIViewModels
{
    public class AddMovieViewModelAPI : BaseAddMovieViewModel
    {
        public IFormFile Image { get; set; }
       
    }

}