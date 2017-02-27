using System;
using System.Collections.Generic;

namespace MediaApplication.Models.MovieViewModels
{

    public class MovieViewModel
    {


        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public GenreViewModel Genre { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public ICollection<DirectorViewModel> Directors { get; set; }
        public ICollection<WriterViewModel> Writers { get; set; }
        public ICollection<StarViewModel> Stars { get; set; }
        public ICollection<ImageViewModel> Images { get; set; }
    }

  public class GenreViewModel
    {
      
        public int Id { get; set; }

      
        public string Title { get; set; }
      
    }

    public class DirectorViewModel
    {
   
        public int Id { get; set; }    
        public string Name { get; set; }       
    }

    public class WriterViewModel
    {
      
        public int Id { get; set; }
      
        public string Name { get; set; }
       

    }

    public class StarViewModel
    {
        
        public int Id { get; set; }
       
        public string Name { get; set; }
     

    }

    public class ImageViewModel {

       
        public int Id { get; set; }
    
        public string Name { get; set; }

        public string OriginalName {get;set;}
        public string Extention {get;set;}
        public string Thumbnail {get;set;}
        public string OriginalImage {get;set;}        
       
    }


    public class PagingMovieViewModel
    {
        public List<MovieViewModel> AllMovies {get;set;}
        public int PageNumber {get;set;}
        public int PageCount {get;set;}

    }

}