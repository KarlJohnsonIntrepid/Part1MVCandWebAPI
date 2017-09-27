using Orchard.Candidate.Net.Areas.Dashboard.Controllers;
using Orchard.Candidate.Net.Infrastructure;
using Orchard.Candidate.Net.Models.Api;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Orchard.Candidate.Net.Controllers.Api
{
    [Authorize]
    public class PostController : ApiController
    {
        private IUserPostApiFacade _userPostsApi { get; set; }

        public PostController()
        {
            //NOTE --  If We had more time this would be dependency injected in the constructor
            _userPostsApi = new UserPostApiFacade();
        }

        /*... Other declarations goes here ...*/
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody]Post post)
        {
            //Check for token - this should be moved in to a filter or message handler, its is left here for demonstration purposes only

            var token = "";
            if (Request.Headers.Contains("Authorization"))
            {
                var authHeader = Request.Headers.GetValues("Authorization").First();
                authHeader = authHeader.Replace("Bearer ", "");

                try
                {
                    //https://stackoverflow.com/questions/7134837/how-do-i-decode-a-base64-encoded-string
                    byte[] data = Convert.FromBase64String(authHeader);
                    token = Encoding.UTF8.GetString(data);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }                
            }

            if(token != "OrchardApiKey")
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized );
            }

            //Validate the request for required title and body (set in the model)
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //Set the user ID to 1, this can be removed at a later date and the id made required and passed in the model
            post.UserId = 1;

            var postId  =  await  _userPostsApi.Post(post);
            if(postId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //Set the Id for post //note this method will also cache the post
            post.Id = postId.GetValueOrDefault();

     

            return Request.CreateResponse(HttpStatusCode.OK, post.Id);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);

        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);

        }
        [HttpPut]
        public async Task<HttpResponseMessage>  Update([FromBody]Post post, int Id)
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }
    }
}
