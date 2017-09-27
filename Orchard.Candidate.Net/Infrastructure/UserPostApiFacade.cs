using Newtonsoft.Json;
using Orchard.Candidate.Net.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Orchard.Candidate.Net.Infrastructure
{
    /// <summary>
    /// This class would be moved elsewhere but is left here for demonstation purposes
    /// </summary>
    public interface IUserPostApiFacade
    {
        HttpClient CreateHttpClient();
        Task<User> GetUser(int? id);
        Task<List<Post>> GetUserPosts(int? id);
        Task<int?> Post(Post post);
    }

    /// <summary>
    /// This class would be moved elsewhere but is left here for demonstation purposes, this class is incomplete only has the functionality required for the test
    /// </summary>
    public class UserPostApiFacade : IUserPostApiFacade
    {
        private IPostCache _postCache { get; set; }

        public UserPostApiFacade()
        {
            //NOTE --  If We had more time this would be dependency injected in the constructor
           
            _postCache = new PostCache();
        }
    

        public HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            return httpClient;

        }

        public async Task<User> GetUser(int? id)
        {
            var httpClient = CreateHttpClient();

            using (httpClient)
            {
                HttpResponseMessage response = await httpClient.GetAsync("users/" + id);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(result);
                }

                return null;
            }
        }

        public async Task<List<Post>> GetUserPosts(int? id)
        {
            var httpClient = CreateHttpClient();

            //SMART BUSINESS LOGIC - We only ever call the service once, check for posts by this user
            if (GetInMemoryPosts(id).Count == 0)
            {
                using (httpClient)
                {
                    HttpResponseMessage response = await httpClient.GetAsync("users/" + id + "/posts");
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        //SMART BUSINESS LOGIC
                        var list = JsonConvert.DeserializeObject<List<Post>>(result);

                        //Add all our posts to in memory cache so we dont have to call the service again
                        foreach (var post in list)
                        {
                            _postCache.Add(post);
                        }

                        return list;
                    }

                    return null;
                }
            }
            else
            {
                return GetInMemoryPosts(id);
            }
        }

        public async Task<int?> Post(Post post)
        {
            var httpClient = CreateHttpClient();

            using (httpClient)
            {
                var content = JsonConvert.SerializeObject(post);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);


                HttpResponseMessage response = await httpClient.PostAsync("posts", byteContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var returnObj = JsonConvert.DeserializeObject<PostReturn>(result);

                    //add the post to in memory cache
                    _postCache.Add(post);

                    return returnObj.Id;
                }

                return null;
            }
        }

        public List<Post> GetInMemoryPosts(int? userId)
        {
            return _postCache.Posts().Where(x => x.UserId == userId).ToList();
        }

    }

    public class PostReturn
    {
        public int? Id { get; set; }
    }
}