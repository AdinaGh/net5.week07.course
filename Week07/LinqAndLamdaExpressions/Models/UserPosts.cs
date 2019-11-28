using System.Collections.Generic;

namespace LinqAndLamdaExpressions.Models
{
    public class UserPosts
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
