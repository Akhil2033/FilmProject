using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmProject.Models.ViewModels
{
    public class DetailsActors
    {
        public ActorDto SelectedActor { get; set; }
        public IEnumerable<FilmDto> StarFilms { get; set; }
    }
}