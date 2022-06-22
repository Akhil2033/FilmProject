using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmProject.Models.ViewModels
{
    public class UpdateFilm
    {
        public FilmDto SelectedFilm { get; set; }

        public IEnumerable<StudioDto> StudioOptions { get; set; }
    }
}