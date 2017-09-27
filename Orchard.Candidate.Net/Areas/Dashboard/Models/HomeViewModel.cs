using Orchard.Candidate.Net.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.Candidate.Net.Areas.Dashboard.Models
{
    public class HomeViewModel
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}