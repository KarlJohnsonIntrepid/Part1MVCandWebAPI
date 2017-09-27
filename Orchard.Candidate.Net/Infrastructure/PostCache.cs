using Orchard.Candidate.Net.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.Candidate.Net.Infrastructure
{

    public interface IPostCache
    {
        List<Post> Posts();
        void Add(Post post);
    }

    public class PostCache : IPostCache
    {
        public List<Post> Posts()
        {
                if (System.Web.HttpRuntime.Cache["Posts"] == null)
                {
                    System.Web.HttpRuntime.Cache["Posts"] = new List<Post>();
                }

                return (List<Post>)System.Web.HttpRuntime.Cache["Posts"];
        }

        public void Add(Post post)
        {
            if (System.Web.HttpRuntime.Cache["Posts"] == null)
            {
                System.Web.HttpRuntime.Cache["Posts"] = new List<Post>();
            }

           var posts = (List<Post>)System.Web.HttpRuntime.Cache["Posts"];
           posts.Add(post);

            System.Web.HttpRuntime.Cache["Posts"] = posts;
        }
    }
}