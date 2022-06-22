using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FilmProject.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int FilmYear { get; set; }
        public string DirectorName { get; set; }
        public string FilmPlot { get; set; }

        //A film is produced by a studio 
        //A Studio produces many films
        [ForeignKey("Studio")]
        public int StudioId { get; set; }
        public virtual Studio Studio { get; set; }

        public ICollection<Actor> Actors { get; set; }

    }

    public class FilmDto
    {
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int FilmYear { get; set; }
        public string DirectorName { get; set; }
        public string FilmPlot { get; set; }

        public int StudioId { get; set; }
        public string StudioName { get; set; }
    }
}