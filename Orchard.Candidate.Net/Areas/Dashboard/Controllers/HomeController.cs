using Newtonsoft.Json;
using Orchard.Candidate.Net.Areas.Dashboard.Models;
using Orchard.Candidate.Net.Filters;
using Orchard.Candidate.Net.Models.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Web.Helpers;
using Orchard.Candidate.Net.Infrastructure;

namespace Orchard.Candidate.Net.Areas.Dashboard.Controllers
{
    [RouteArea("Dashboard")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserPostApiFacade _userPostsApi { get; set; }

        public  HomeController()
        {
            //NOTE --  If We had more time this would be dependency injected in the contsructor
            _userPostsApi = new UserPostApiFacade();
        }


        [CustomActionFilter]
        public async Task<ActionResult> Index(int? id)
        {
            //Load First 5 posts
            var posts = await _userPostsApi.GetUserPosts(id);
            posts = posts.OrderByDescending(x=> x.Id).Take(5).ToList();

            var model = new HomeViewModel()
            {
                User = await _userPostsApi.GetUser(id),
                Posts = posts
            };

            return View(model);
        }


        [HttpPost]
        [CustomActionFilter]
        public async Task<ActionResult> GetOlderPosts(int? id, int page)
        {
            var posts = await _userPostsApi.GetUserPosts(id);

            posts = posts.OrderByDescending(x=> x.Id).Skip(5 * page).Take(5).ToList();

            return Json(JsonConvert.SerializeObject(posts));
        }
    }
}