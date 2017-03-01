using Microsoft.AspNetCore.Mvc;
using System;
using MediaApplication.Models.MovieViewModels;
using System.Collections.Generic;
using MediaApplication.Models;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using MediaApplication.Services;
using NLog;
using Microsoft.AspNetCore.Authorization;
using MediaApplication.Common;

public class MoviesController : Controller
{

    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly CommonFunction _commonFunction;

    public MoviesController(IMapper mapper, IHostingEnvironment hostingEnvironment, IMovieService movieService, CommonFunction commonFunction)
    {
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
        _movieService = movieService;
        _commonFunction = commonFunction;

    }


    public IActionResult Index(int pageNumber = 1)
    {
        int pageSize = 10;
        PagingMovieViewModel movies = new PagingMovieViewModel();

        try
        {
            _logger.Info("Index Page invoked");
            var movieList = _movieService.GetAllMovies(pageSize: pageSize, pageNumber: pageNumber);
            var allMovies = _mapper.Map<List<Movie>, List<MovieViewModel>>(movieList);
            int totalMovies = allMovies.Count;
            int pageCount = totalMovies % pageSize == 0 ? totalMovies / pageSize : totalMovies / pageSize + 1;
            movies.PageNumber = pageNumber;
            movies.PageCount = pageCount;
            movies.AllMovies = allMovies;

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occured");
            _commonFunction.LogException(ex);
        }

        return View(movies);
    }

    [HttpGet]
    //[Authorize(ActiveAuthenticationSchemes = "CookieAuth")]
    [Authorize(ActiveAuthenticationSchemes = "Identity.Application")]
    //[ValidateAntiForgeryToken]
    public IActionResult Add()
    {
        AddMovieViewModel model = new AddMovieViewModel();
        try
        {
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occured");
            _commonFunction.LogException(ex);

        }
        // model.AllGenre = GetAllGenre();
        return View(model);
    }

    [HttpPost]
    [Authorize(ActiveAuthenticationSchemes = "Identity.Application")]
    // [Authorize(ActiveAuthenticationSchemes = "CookieAuth")]
    [ValidateAntiForgeryToken]
    public IActionResult Add([Bind("Title", "Description", "ReleaseDate", "Director", "Writer", "MovieStars", "files", "GenreId")]AddMovieViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            else if (ModelState.IsValid)
            {
                DateTime dateValue;
                if (!DateTime.TryParse(Convert.ToString(model.ReleaseDate), out dateValue))
                {
                    return ReturnViewWithModelState(model, "ReleaseDate", "Please enter valid release date");

                }
                else
                {
                    if (model.ReleaseDate.Date < DateTime.Now)
                    {
                        return ReturnViewWithModelState(model, "ReleaseDate", "Release date should be greater than or equal to today");

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
                            return ReturnViewWithModelState(model, "files", "Please upload valid file");
                        }
                        string fileExt = segments[1];
                        String[] extentionArr = new string[] { "jpg", "png", "jpeg", "gif", "bmp", "tif" };

                        if (Array.IndexOf(extentionArr, fileExt.ToLower()) < 0)
                        {
                            return ReturnViewWithModelState(model, "files", "Please upload valid file (only images are allowed)");
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
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            _commonFunction.LogException(ex);
            return View(model);

        }
        return View(model);


    }


    //movie detail

    public IActionResult detail(int? id)
    {

        if (id == null)
        {
            return RedirectToAction("Index");
        }
        var movie = _movieService.GetMovieDetails(Convert.ToInt32(id));

        if (movie == null || movie.Id == 0)
        {
            return RedirectToAction("Index");
        }
        //return Content("test" + id.ToString());
        MovieViewModel viewModel =  new   MovieViewModel();
    
        viewModel =  _mapper.Map<Movie, MovieViewModel>(movie);
        return View(viewModel);
    }


    #region private methods




    #region return view with model
    private ViewResult ReturnViewWithModelState(AddMovieViewModel model, string key, string errorMessage)
    {
        ModelState.AddModelError(key, errorMessage);
        return View();
    }
    #endregion

    #endregion

}