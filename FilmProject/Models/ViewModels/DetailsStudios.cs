using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmProject.Models.ViewModels
{
    public class DetailsStudios
    {
        public StudioDto SelectedStudio { get; set; }
        public IEnumerable<FilmDto> RelatedFilms { get; set; }
    }
}