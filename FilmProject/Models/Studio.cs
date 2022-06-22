using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace FilmProject.Models
{
    public class Studio
    {
        [Key]
        public int StudioId { get; set; }
        public string StudioName { get; set; }
        public string StudioDesc { get; set; }


    }

    public class StudioDto
    {
        public int StudioId { get; set; }
        public string StudioName { get; set; }
        public string StudioDesc { get; set; }

    }
}