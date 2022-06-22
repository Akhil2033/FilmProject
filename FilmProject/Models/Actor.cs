using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FilmProject.Models
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public int ActorFee { get; set; }


        // An actor can work on many films

        public ICollection<Film> Films { get; set; }

    }

    public class ActorDto
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public int ActorFee { get; set; }
      

    }
}