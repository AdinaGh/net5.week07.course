namespace LinqAndLamdaExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var allUsers = ReadUsers("users.json");
            var allPosts = ReadPosts("posts.json");

            // 1 - find all users having email ending with ".net".
            var users1 = from u in allUsers
                where u.Email.EndsWith(".net")
                select u;

            var users2 = allUsers.Where(x => x.Email.EndsWith(".net"));

            var emails = allUsers.Select(x => x.Email).ToList();

            // 2 - find all posts for users having email ending with ".net".

            IEnumerable<int> usersIdsWithDotNetMails = from user in allUsers
                                                      where user.Email.EndsWith(".net")
                                                      select user.Id;

            IEnumerable<Post> posts = from post in allPosts
                                      where usersIdsWithDotNetMails.Contains(post.UserId)
                                      select post;

            foreach (var post in posts)
            {
                Console.WriteLine(post.Id + " " + "user: " + post.UserId);
            }
            // 3 - print number of posts for each user.

            var nrPosts = from p in allPosts
                          group p by p.UserId into grp
                          select new { 
                              UserId = grp.Key, 
                              Count = grp.Count() 
                          };

            // 4 - find all users that have lat and long negative.

            var neg = from u in allUsers
                      where u.Address?.Geo?.Lat < 0 && u.Address?.Geo?.Lng < 0
                      select u;

            // 5 - find the post with longest body.

            var bodyMax = allPosts.Max(p => p.Body);


            // 6 - print the name of the employee that have post with longest body.

            var nameBody = (from p in allPosts
                           join u in allUsers on p.UserId equals u.Id
                           where p.Body == bodyMax
                           select u.Name).FirstOrDefault();

            Console.WriteLine(nameBody);

            // 7 - select all addresses in a new List<Address>. print the list.

            var listaAddress = (from u in allUsers
                              select u.Address).ToList();


            listaAddress.ForEach(ad =>
            {
                Console.WriteLine($"{ ad.Street} {ad.Suite} {ad.City} {ad.Zipcode}");
            });

            // 8 - print the user with min lat

            var latMin = allUsers.Min(u => u.Address?.Geo?.Lat);

            Console.WriteLine(latMin);

            // 9 - print the user with max long

            var longMax = allUsers.Max(u => u.Address?.Geo?.Lng);

            Console.WriteLine(longMax);


            // 10 - create a new class: public class UserPosts { public User User {get; set}; public List<Post> Posts {get; set} }
            //    - create a new list: List<UserPosts>
            //    - insert in this list each user with his posts only

            var userPosts = allUsers.Select(u => new UserPosts { User = u, Posts = allPosts.Where(p => p.UserId == u.Id).ToList() }).ToList();

            // 11 - order users by zip code 

            var ordUser1 = allUsers.OrderBy(u => u.Address?.Zipcode);


            // 12 - order users by number of posts

            var oderUser2 = allUsers.OrderBy(u => allPosts.Where(p => p.UserId == u.Id).Count());
        }

        private static List<Post> ReadPosts(string file)
        {
            return ReadData.ReadFrom<Post>(file);
        }

        private static List<User> ReadUsers(string file)
        {
            return ReadData.ReadFrom<User>(file);
        }
    }
}
