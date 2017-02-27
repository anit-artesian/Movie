using System;
using System.Collections.Generic;
using AutoMapper;
using MediaApplication.Common;
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
        private readonly CommonFunction _commonFunction;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public MovieAPIController(IMovieService movieService, IMapper mapper, CommonFunction commonFunction)
        {
            _movieService = movieService;
            _mapper = mapper;
            _commonFunction = commonFunction;

        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult GetAllMovies()
        {

            _logger.Info("API GetMovies action invoked...");
            List<MovieViewModel> allMovies = new List<MovieViewModel>();
            try
            {
                var movieList = _movieService.GetAllMovies(pageSize:10,pageNumber:1);
                allMovies = _mapper.Map<List<Movie>, List<MovieViewModel>>(movieList);
                return Ok(allMovies);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.GetBaseException().Message + ex.GetBaseException().InnerException + ex.GetBaseException().StackTrace);

            }

            return Ok(allMovies);
        }

        [HttpPost]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult AddMovie([Bind("Title", "Description", "ReleaseDate", "Director", "Writer", "MovieStars", "files", "GenreId")]AddMovieViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new Dictionary<string, string>();
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(state.Key, error.ErrorMessage);
                        }
                    }
                    APIResponse responseError = new APIResponse
                    {
                        Message = "The request is invalid.",
                        Error = errors

                    };
                    return BadRequest(responseError);
                }

                DateTime dateValue;
                if (!DateTime.TryParse(Convert.ToString(model.ReleaseDate), out dateValue))
                {
                    return ReturnBadRequest("ReleaseDate", "Please enter valid release date");
                }
                else
                {
                    if (model.ReleaseDate.Date < DateTime.Now)
                    {
                        return ReturnBadRequest("ReleaseDate", "Release date should be greater than or equal to today");
                    }
                }

                // Map fields using AutoMapper
                Movie movie = _mapper.Map<Movie>(model);

                if (model.files != null)
                {
                    var file = model.files;

                    if (file.Length > 0)
                    {
                        string[] segments = file.FileName.Split('.');
                        if (segments.Length == 1)
                        {
                            return ReturnBadRequest("files", "Release date should be greater than or equal to today");
                        }
                        string fileExt = segments[1];
                        String[] extentionArr = new string[] { "jpg", "png", "jpeg", "gif", "bmp", "tif" };

                        if (Array.IndexOf(extentionArr, fileExt.ToLower()) < 0)
                        {
                            return ReturnBadRequest("files", "Please upload valid file (only images are allowed)");
                        }

                        // Process uploaded image
                        movie.Images = _commonFunction.ProcessUploadedImage(file, movie.Id, fileExt, segments);
                    }

                }


                if (!String.IsNullOrEmpty(model.Director))
                {
                    movie.Directors = _commonFunction.GetMovieDirectors(model.Director, movie.Id);
                }


                if (!String.IsNullOrEmpty(model.Writer))
                {
                    movie.Writers = _commonFunction.GetMovieWriters(model.Writer, movie.Id);
                }

                if (!String.IsNullOrEmpty(model.MovieStars))
                {
                    movie.Stars = _commonFunction.GetMovieStars(model.MovieStars, movie.Id); ;
                }

                // Add movie
                _movieService.AddMovie(movie);

                APIResponse apiResponse = new APIResponse
                {
                    Message = "Movie Added!!!",
                    Error = null
                };
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured while logging exception");
                return BadRequest("some error occured. Please try again.");

            }

        }

        [HttpGet("genre")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult GetGenreList()
        {
            try
            {
                var genreList = _movieService.GetGenre();
                return Ok(genreList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured while logging exception");
                return BadRequest("some error occured. Please try again.");
            }

        }


        private IActionResult ReturnBadRequest(string key, string value)
        {

            APIResponse response = new APIResponse
            {
                Message = "The request is invalid.",
                Error = new Dictionary<string, string>()
                {
                    {
                       key,value
                    }
                }
            };

            return BadRequest(response);

        }




    }

}