using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MediaApplication.Models.MovieViewModels;
using System.Collections.Generic;

using MediaApplication.Models;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MediaApplication.Services;
using NLog;
using Microsoft.AspNetCore.Authorization;

public class MoviesController : Controller
{

    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public MoviesController(IMapper mapper, IHostingEnvironment hostingEnvironment, IMovieService movieService)
    {
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
        _movieService = movieService;

    }


    public IActionResult Index()
    {
        List<AllMovieViewModel> allMovies = new List<AllMovieViewModel>();
        try
        {
            _logger.Info("Index Page invoked");
            var movieList = _movieService.GetAllMovies();
            allMovies = _mapper.Map<List<Movie>, List<AllMovieViewModel>>(movieList);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occured");
            LogException(ex);
        }

        return View(allMovies);
    }

    [HttpGet]
    public IActionResult Add()
    {
        AddMovieViewModel model = new AddMovieViewModel();
        try
        {

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occured");
            LogException(ex);
        }
        // model.AllGenre = GetAllGenre();
        return View(model);
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
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
                        movie.Images = ProcessUploadedImage(file, movie.Id, fileExt, segments);
                    }

                }


                if (!String.IsNullOrEmpty(model.Director))
                {
                    movie.Directors = GetMovieDirectors(model.Director, movie.Id);
                }


                if (!String.IsNullOrEmpty(model.Writer))
                {
                    movie.Writers = GetMovieWriters(model.Writer, movie.Id);
                }

                if (!String.IsNullOrEmpty(model.MovieStars))
                {
                    movie.Stars = GetMovieStars(model.MovieStars, movie.Id); ;
                }

                // Add movie
                _movieService.AddMovie(movie);
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            LogException(ex);
            return View(model);

        }
        return View(model);


    }

    #region private methods

    #region log exception
    private void LogException(Exception ex)
    {
        try
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var logFilePath = Path.Combine(webRootPath, "Logfiles");
            Random rnd = new Random();
            string random = rnd.Next(10000, 999999999).ToString();
            string fileName = random + "_" + "log.txt";
            string filepath = Path.Combine(logFilePath, fileName);

            using (FileStream fs = System.IO.File.Create(filepath))
            {
                // writing data in string
                string errorMessage = DateTime.Now.ToString();
                errorMessage += Convert.ToString(ex.Message) + Convert.ToString(ex.GetBaseException().Message) + Convert.ToString(ex.StackTrace);
                errorMessage += Environment.NewLine;
                errorMessage += Environment.NewLine;
                errorMessage += Environment.NewLine;

                byte[] info = new UTF8Encoding(true).GetBytes(errorMessage);
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception exception)
        {

            _logger.Error(exception, "Error occured while logging exception");
        }
    }

    #endregion

    #region create image thumbnail

    private string CreateThumbnail(MemoryStream ms)
    {
        string imageBaseString = String.Empty;
        try
        {
            System.Drawing.Image createdImage = System.Drawing.Image.FromStream(ms);
            System.Drawing.Image thumbnailImage = createdImage.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
            byte[] byteArr = ImageToByteArray(thumbnailImage);
            imageBaseString = Convert.ToBase64String(byteArr);
            return imageBaseString;
        }
        catch (Exception ex)
        {
            LogException(ex);
            imageBaseString = String.Empty;

        }
        return imageBaseString;


    }

    #endregion

    #region
    private byte[] ImageToByteArray(System.Drawing.Image imageIn)
    {
        using (var ms = new MemoryStream())
        {
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
    #endregion

    #region
    private List<Image> ProcessUploadedImage(IFormFile file, int movieId, string fileExt, string[] segments)
    {
        List<Image> movieImagesList = new List<Image>();
        try
        {
            Random rnd = new Random();
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string imageBaseString = Convert.ToBase64String(fileBytes);

                    // create thumbnail and get base 64 string
                    string thumbnailBaseString = CreateThumbnail(ms);


                    Image image = new Image();
                    image.MovieId = movieId;


                    string originalFileName = segments[0];

                    image.OriginalName = originalFileName;
                    image.Extention = fileExt;
                    image.OriginalImage = imageBaseString;

                    string random = rnd.Next(1000000, 9999999).ToString();
                    string fileName = random + "_" + segments[0];
                    image.Name = fileName;
                    image.Thumbnail = thumbnailBaseString;
                    movieImagesList.Add(image);
                }
            }
        }
        catch (Exception ex)
        {
            LogException(ex);

        }
        return movieImagesList;


    }
    #endregion

    #region Get movie Directors
    private List<Director> GetMovieDirectors(string movieDirectors, int movieId)
    {
        List<Director> directors = new List<Director>();
        try
        {
            foreach (var director in movieDirectors.Split(','))
            {
                directors.Add(new Director()
                {
                    Name = director,
                    MovieId = movieId

                });

            }
        }
        catch (Exception ex)
        {
            LogException(ex);

        }
        return directors;

    }
    #endregion

    #region Get movie Writers
    private List<Writer> GetMovieWriters(String movieWriters, int movieId)
    {
        List<Writer> writers = new List<Writer>();
        try
        {
            foreach (var writer in movieWriters.Split(','))
            {
                writers.Add(new Writer()
                {
                    Name = writer,
                    MovieId = movieId

                });
            }
        }
        catch (Exception ex)
        {
            LogException(ex);

        }
        return writers;

    }
    #endregion

    #region Get movie Stars
    private List<Star> GetMovieStars(String movieStars, int movieId)
    {
        List<Star> stars = new List<Star>();
        try
        {
            foreach (var star in movieStars.Split(','))
            {
                stars.Add(new Star()
                {
                    Name = star,
                    MovieId = movieId

                });
            }
        }
        catch (Exception ex)
        {
            LogException(ex);

        }
        return stars;

    }
    #endregion


    #region return view with model
    private ViewResult ReturnViewWithModelState(AddMovieViewModel model, string key, string errorMessage)
    {
        ModelState.AddModelError(key, errorMessage);
        return View();
    }
    #endregion


    #endregion

}