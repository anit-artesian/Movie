using System;
using System.ComponentModel.DataAnnotations;

namespace MediaApplication.Models.CommonMovieViewModel
{

    public class BaseAddMovieViewModel
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

    }
}