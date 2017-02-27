using System;
using System.ComponentModel.DataAnnotations;
using MediaApplication.Models.CommonMovieViewModel;
using Microsoft.AspNetCore.Http;

namespace MediaApplication.Models.MovieViewModels
{

    public class AddMovieViewModel 
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [RegularExpression(@"^.{1,}$", ErrorMessage = "Minimum 5 characters required")]
        [StringLength(30, ErrorMessage = "Title should be maximium 30 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
         [StringLength(500, ErrorMessage = "Description should be maximium 30 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Director is Required")]
        public string Director { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please select Genre")]
        public int GenreId { get; set; }

        public string Writer { get; set; }

        public string MovieStars { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter valid Release date")]
        public DateTimeOffset ReleaseDate { get; set; }


        public GenreEnum AllGenre { get; set; }
        public IFormFile files { get; set; }
        public AddMovieViewModel()
        {
            this.ReleaseDate = DateTime.Now.AddDays(1);
        }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (ReleaseDate.Date < DateTime.Now.Date)
        //     {

        //         yield return
        //             new ValidationResult(errorMessage: "ReleaseDate should be greate than or equal to today",
        //                           memberNames: new[] { "ReleaseDate" });
        //     }
        // }
    }



}