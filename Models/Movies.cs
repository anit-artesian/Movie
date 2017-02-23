using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaApplication.Models
{

    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }

    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }      

        [Required]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }

        [Required]
        public DateTimeOffset ReleaseDate { get; set; }

        public ICollection<Director> Directors { get; set; }
        public ICollection<Writer> Writers { get; set; }
        public ICollection<Star> Stars { get; set; }
        public ICollection<Image> Images { get; set; }
    }

    public class Director
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }

    public class Writer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

    }

    public class Star
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

    }

    public class Image {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string OriginalName {get;set;}
        public string Extention {get;set;}
        public string Thumbnail {get;set;}
        public string OriginalImage {get;set;}
        
        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }

}