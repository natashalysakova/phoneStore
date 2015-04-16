﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ReccomendationController : Controller
    {
        storeDBEntities Entities = new storeDBEntities();
        // GET: Reccomendation
        public ActionResult Index()
        {RecomendationModel model = new RecomendationModel();
            model.Users = Entities.Users.ToList();
            model.Liknesses = GetListOfSimilarUsers();
            model.PersonalRecomendation = GetRecomendation(model.Liknesses);

            return View(model);
        }

        private List<SimilarUsers> GetListOfSimilarUsers()
        {
            List<SimilarUsers> list = new List<SimilarUsers>();
            List<Users> users = new List<Users>();

            foreach (Users user in Entities.Users)
            {
                foreach (Users otherUser in Entities.Users)
                {
                    if (user.Name == otherUser.Name)
                    {
                        list.Add(new SimilarUsers() { First = user, Second = otherUser, SimilarCoef = 0 });
                    }
                    else
                    {
                        int togetherApps = (from userScore in user.Scores
                                            from otherScores in otherUser.Scores
                                            where userScore.Phones == otherScores.Phones
                                            select userScore.Phones).Count();

                        double coef = togetherApps / (user.Scores.Count + otherUser.Scores.Count - togetherApps);
                        list.Add(new SimilarUsers() { First = user, Second = otherUser, SimilarCoef = coef });
                    }
                }

                users.Add(user);
            }

            return list;
        }

        public List<Phones> GetRecomendation(List<SimilarUsers> list)
        {
            List<Phones> personal = new List<Phones>();
            if (User.Identity.IsAuthenticated)
            {
                Users user = Entities.Users.SingleOrDefault(x => x.Name == User.Identity.Name);
                if (user != null)
                {
                    List<SimilarUsers> usersLikeMe = new List<SimilarUsers>();
                    double maxSimilarUsers = 0;
                    foreach (SimilarUsers similarUsers in list.Where(x => x.First == user))
                    {
                        usersLikeMe.Add(similarUsers);
                        if (similarUsers.SimilarCoef > maxSimilarUsers)
                        {
                            maxSimilarUsers = similarUsers.SimilarCoef;
                        }
                    }

                    usersLikeMe =
                        usersLikeMe.OrderByDescending(x => x.SimilarCoef)
                            .Where(x => (x.SimilarCoef >= (0.75 * maxSimilarUsers)))
                            .ToList();

                    List<Phones> userApps = (from scores in user.Scores select scores.Phones).ToList();

                    foreach (SimilarUsers similarUsers in usersLikeMe)
                    {
                        List<Phones> otherUserApps = (from scores in similarUsers.Second.Scores where scores.Result > 4 select scores.Phones).ToList();

                        personal.AddRange(from apps in otherUserApps where !userApps.Contains(apps) select apps);
                    }
                }
            }
            return personal;
        }

        public class SimilarUsers
        {
            public Users First { get; set; }
            public Users Second { get; set; }
            public double SimilarCoef { get; set; }
        }

        public class RecomendationModel
        {
            public List<SimilarUsers> Liknesses { get; set; }
            public List<Users> Users { get; set; }
            public List<Phones> PersonalRecomendation { get; set; }
        }

    }
}