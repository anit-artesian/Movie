using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediaApplication.Models;
using MediaApplication.Models.MovieViewModels;
using MediaApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace MediaApplication.Controllers.API
{
    [Route("api/[controller]")]
    public class MovieAPIController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public MovieAPIController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;

        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllMovies()
        {
             _logger.Info("API GetMovies action invoked...");
            List<AllMovieViewModel> allMovies = new List<AllMovieViewModel>();
            try
            {
                var movieList = _movieService.GetAllMovies();
                allMovies = _mapper.Map<List<Movie>, List<AllMovieViewModel>>(movieList);
                return Ok(allMovies);
          
            }
            catch (Exception ex)
            {
               
                _logger.Error(ex, ex.GetBaseException().Message + ex.GetBaseException().InnerException + ex.GetBaseException().StackTrace);

            }
            return Ok(allMovies);
        }
    }

}