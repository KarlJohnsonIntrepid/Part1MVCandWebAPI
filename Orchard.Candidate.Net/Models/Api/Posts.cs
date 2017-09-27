using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orchard.Candidate.Net.Models.Api
{

    public class Post
    {//Full class shoudl be implemented, but due to time constraint is not

        public int UserId { get; set; }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }

    }
}