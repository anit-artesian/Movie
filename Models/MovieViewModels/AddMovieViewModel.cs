using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MediaApplication.Models.MovieViewModels
{

    public class AddMovieViewModel
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
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





        public IFormFile files { get; set; }

        public GenreEnum AllGenre { get; set; }




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