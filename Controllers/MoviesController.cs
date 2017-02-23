using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MediaApplication.Data;
using MediaApplication.Models.MovieViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediaApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;

public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHostingEnvironment _hostingEnvironment;

    public MoviesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;

    }
    public IActionResult Index()
    {

        var allMovies = _context.Movies.Include(c => c.Directors).Include(c => c.Writers).Include(c => c.Stars).Include(c => c.Images).OrderByDescending(x => x.ReleaseDate).ToList();

        return View(allMovies);
    }

    [HttpGet]
    public IActionResult Add()
    {
        AddMovieViewModel model = new AddMovieViewModel();
        model.AllGenre = GetAllGenre();
        return View(model);
    }

    [HttpPost]
    public IActionResult Add(AddMovieViewModel model)
    {
        try
        {

            if (ModelState.IsValid)
            {
                DateTime dateValue;
                if (!DateTime.TryParse(Convert.ToString(model.ReleaseDate), out dateValue))
                {

                    ModelState.AddModelError("ReleaseDate", "Please enter valid release date");
                    model.AllGenre = GetAllGenre();
                    return View(model);
                }


                else
                {
                    if (model.ReleaseDate.Date < DateTime.Now)
                    {
                        ModelState.AddModelError("ReleaseDate", "Release date should be greater than or equal to today");
                        model.AllGenre = GetAllGenre();
                        return View(model);

                    }

                }

                Movie movie = new Movie();

                Random rnd = new Random();
                if (model.files != null)
                {
                    List<Image> movieImagesList = new List<Image>();
                    if (model.files != null)
                    {
                        var file = model.files;

                        if (file.Length > 0)
                        {

                            string[] segments = file.FileName.Split('.');
                            if (segments.Length ==1)
                            {
                                ModelState.AddModelError("files", "Please upload valid files (only images are allowed)");
                                model.AllGenre = GetAllGenre();
                                return View(model);

                            }
                            string fileExt = segments[1];
                            String[] extentionArr = new string[] { ".jpg", ".png", ".jpeg", ".gif",".bmp",".tif" };
                            if (Array.IndexOf(extentionArr, fileExt.ToLower()) < 0)
                            {

                                ModelState.AddModelError("files", "Please upload valid files (only images are allowed)");
                                model.AllGenre = GetAllGenre();
                                return View(model);

                            }

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
                                    image.MovieId = movie.Id;


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
                    }

                    movie.Images = movieImagesList;
                }


                // var allDirector = model.Director.Split(',').ToList();
                // var allWriter = model.Writer.Split(',').ToList();
                movie.Title = model.Title;
                movie.Description = model.Description;
                //   DateTimeOffset parsedDate;
                movie.ReleaseDate = model.ReleaseDate;

                movie.GenreId = Convert.ToInt32(model.SelectedGenre);

                List<string> movieDirectors = new List<string>();

                if (!String.IsNullOrEmpty(model.Director))
                {
                    movieDirectors = model.Director.Split(',').ToList();
                }

                List<Director> directors = new List<Director>();
                foreach (var director in movieDirectors)
                {
                    directors.Add(new Director()
                    {
                        Name = director,
                        MovieId = movie.Id

                    });

                }


                List<string> movieWriter = new List<string>();

                if (!String.IsNullOrEmpty(model.Writer))
                {
                    movieWriter = model.Writer.Split(',').ToList();
                }

                List<Writer> writers = new List<Writer>();
                foreach (var writer in movieWriter)
                {
                    writers.Add(new Writer()
                    {
                        Name = writer,
                        MovieId = movie.Id

                    });

                }




                List<string> allStar = new List<string>();
                if (!String.IsNullOrEmpty(model.MovieStars))
                {

                    allStar = model.MovieStars.Split(',').ToList();
                }


                List<Star> stars = new List<Star>();
                foreach (var star in allStar)
                {
                    stars.Add(new Star()
                    {
                        Name = star,
                        MovieId = movie.Id

                    });

                }

                movie.Stars = stars;
                movie.Writers = writers;
                movie.Directors = directors;






                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            LogException(ex);
            model.AllGenre = GetAllGenre();
            return View(model);

        }

        model.AllGenre = GetAllGenre();
        return View(model);
    }

    #region private methods

    private List<SelectListItem> GetAllGenre()
    {
        var genreList = new List<SelectListItem>();
        var genres = _context.Genres;
        foreach (var genre in genres)
        {
            genreList.Add(new SelectListItem()
            {
                Text = genre.Title,
                Value = genre.Id.ToString()

            });

        }
        return genreList;

    }
    #endregion


    #region log exception
    private void LogException(Exception ex)
    {
        string webRootPath = _hostingEnvironment.WebRootPath;
        var logFilePath = Path.Combine(webRootPath, "Logfiles");
        Random rnd = new Random();
        string random = rnd.Next(1000000, 9999999).ToString();
        string fileName = random + "_" + "log.txt";
        string filepath = Path.Combine(logFilePath, fileName);

        using (FileStream fs = System.IO.File.Create(filepath))
        {
            // writing data in string
            string errorMessage = DateTime.Now.ToString();
            errorMessage += Convert.ToString(ex.Message) + Convert.ToString(ex.InnerException) + Convert.ToString(ex.StackTrace);
            errorMessage += Environment.NewLine;
            errorMessage += Environment.NewLine;
            errorMessage += Environment.NewLine;

            byte[] info = new UTF8Encoding(true).GetBytes(errorMessage);
            fs.Write(info, 0, info.Length);
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
}