using System;
using System.Collections.Generic;

namespace MediaApplication.Models.MovieViewModels
{

    public class AllMovieViewModel
    {


        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public Genre Genre { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public ICollection<Director> Directors { get; set; }
        public ICollection<Writer> Writers { get; set; }
        public ICollection<Star> Stars { get; set; }
        public ICollection<Image> Images { get; set; }
    }

}