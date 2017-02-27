using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MediaApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NLog;

namespace MediaApplication.Common
{
    public class CommonFunction
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public CommonFunction(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
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
                imageBaseString = String.Empty;
                _logger.Error(ex, "Error occured while logging exception");

            }
            return imageBaseString;


        }

        #region
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        #endregion

        #region Get movie Directors
        public List<Director> GetMovieDirectors(string movieDirectors, int movieId)
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
                _logger.Error(ex, "Error occured while logging exception");
            }
            return directors;

        }
        #endregion

        #region Get movie Writers
        public List<Writer> GetMovieWriters(String movieWriters, int movieId)
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

                _logger.Error(ex, "Error occured while logging exception");
            }
            return writers;

        }
        #endregion





        #region Get movie Stars
        public List<Star> GetMovieStars(String movieStars, int movieId)
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
                _logger.Error(ex, "Error occured while logging exception");

            }
            return stars;

        }
        #endregion


        #region
        public List<Image> ProcessUploadedImage(IFormFile file, int movieId, string fileExt, string[] segments)
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
                _logger.Error(ex, "Error occured while logging exception");

            }
            return movieImagesList;


        }
        #endregion

        #region log exception
        public void LogException(Exception ex)
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


    }
}