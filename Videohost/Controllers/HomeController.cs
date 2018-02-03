using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Videohost.Models;

namespace Videohost.Controllers
{
    public class HomeController : Controller
    {
        VideohostEntities db = new VideohostEntities();

        public ActionResult Index(int? id)
        {
            string Game;
            if(id == null)
            {
                id = 1;
                Game = "Dota 2";
            } else if(id == 2)
            {
                Game = "CS:GO";
            } else if(id == 3)
            {
                Game = "Heartstone";
            } else
            {
                Game = "PUBG";
            }
            ViewBag.Game = Game;
            var tournaments = db.Tournaments.Where(p => p.id_game == id);
            ViewBag.Videos = db.Videos.Where(p => p.id_game == id);//.Where(p => p.quantity_like > 999);
            foreach (var item in ViewBag.Videos)
            {
                ViewBag.first_video = "https://www.youtube.com/embed/" + item.link_video.Substring(32);
                break;
            }
            ViewBag.News = db.News.Where(p => p.id_game == id).OrderByDescending(p => p.Id);
            return View(tournaments);
        }

        public void ProfileData()
        {
                string name = User.Identity.Name;
            User user = db.Users.FirstOrDefault(p => p.Email == name);
                var aboutuser = db.AboutUsers.Where(p => p.Id == user.Id);
                int user_id = 0;
                foreach (var s in aboutuser)
                {
                    user_id = s.Id;
                    ViewBag.ProfilePhoto = s.link_photo;
                    ViewBag.ProfileText = s.about;
                }
                var friend = db.Friends.Where(p => p.id_user == user_id);
                foreach (var s in friend)
                {
                    ViewBag.FriendProfile = db.Users.Where(p => p.Id == s.id_user_2);
                }
                ViewBag.MessageProfile = db.Messages.Where(p => p.id_user == user_id);
                ViewBag.VideoProfile = db.Videos.Where(p => p.id_user == user_id);
                foreach (var s in ViewBag.VideoProfile)
                {
                    ViewBag.first_video_profile = "https://www.youtube.com/embed/" + s.link_video.Substring(32);
                    break;
                }
                ViewBag.Message = db.Messages;
            
        }

        [HttpGet]
        public ActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ProfileData();
                return View();
            }
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Chat")]
        public ActionResult Profile(string message)
        {
            if(User.Identity.IsAuthenticated)
            {
                string date = DateTime.Now.ToString();
                User user = new User();
                string name = User.Identity.Name;
                user = db.Users.FirstOrDefault(p => p.Email == name);
                Message messages = new Message
                {
                    Id = db.Messages.Where(p => p.Id > 1).OrderByDescending(p => p.Id).FirstOrDefault().Id + 1,
                    date = date,
                    id_user = user.Id,
                    text = message
                };
                db.Messages.Add(messages);
                db.SaveChanges();
                ProfileData();
                return View();
            } else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ProfilePhoto(string photo)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                User user = db.Users.FirstOrDefault(p => p.Email == name);
                AboutUser aboutUser = db.AboutUsers.FirstOrDefault(p => p.Id == user.Id);
                if (photo != "")
                {
                    db.AboutUsers.Remove(aboutUser);
                    db.SaveChanges();
                    AboutUser aboutuser = new AboutUser
                    {
                        Id = user.Id,
                        link_photo = photo,
                        about = aboutUser.about
                    };
                    db.AboutUsers.Add(aboutuser);
                    db.SaveChanges();
                }
                ProfileData();
                return RedirectToAction("Profile", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult ProfileAbout(string about)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                User user = db.Users.FirstOrDefault(p => p.Email == name);
                AboutUser aboutUser = db.AboutUsers.FirstOrDefault(p => p.Id == user.Id);
                if (about != "")
                {
                    db.AboutUsers.Remove(aboutUser);
                    db.SaveChanges();
                    AboutUser aboutuser = new AboutUser
                    {
                        Id = user.Id,
                        link_photo = aboutUser.link_photo,
                        about = about
                    };
                    db.AboutUsers.Add(aboutuser);
                    db.SaveChanges();
                }
                ProfileData();
                return RedirectToAction("Profile", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult Addvideo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Game = db.Games;
            return View();
        }

        [HttpPost]
        public ActionResult Addvideo(string link_video, int games)
        {
            if(User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                User user = db.Users.FirstOrDefault(p => p.Email == name);
                Video video = new Video()
                {
                    Id = db.Videos.Count() + 1,
                    link_video = link_video,
                    id_game = games,
                    id_user = user.Id
                };
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Profile", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult News(int id)
        {
            var News = db.News.Where(p => p.Id == id);
            return View(News);
        }
        
        public ActionResult Like(int VideoId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                User user = db.Users.FirstOrDefault(p => p.Email == name);
                Video video = db.Videos.FirstOrDefault(p => p.Id == VideoId);
                LikeDislike like = db.LikeDislikes.FirstOrDefault(p => p.id_user == user.Id && p.id_video == video.Id);
                if (like == null)
                {
                    LikeDislike dislike = new LikeDislike()
                    {
                        Id = db.LikeDislikes.Count() + 1,
                        id_user = user.Id,
                        id_video = video.Id,
                        assessment = 1
                    };
                    db.LikeDislikes.Add(dislike);
                    db.SaveChanges();

                } else if(like.assessment == 2)
                {
                    db.LikeDislikes.Remove(like);
                    LikeDislike likedislike = new LikeDislike()
                    {
                        Id = like.Id,
                        id_user = user.Id,
                        id_video = video.Id,
                        assessment = 1
                    };
                    db.LikeDislikes.Add(likedislike);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dislike(int VideoId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                User user = db.Users.FirstOrDefault(p => p.Email == name);
                Video video = db.Videos.FirstOrDefault(p => p.Id == VideoId);
                LikeDislike dislike = db.LikeDislikes.FirstOrDefault(p => p.id_user == user.Id && p.id_video == video.Id);
                if (dislike == null)
                {
                    LikeDislike likedislike = new LikeDislike()
                    {
                        Id = db.LikeDislikes.Count() + 1,
                        id_user = user.Id,
                        id_video = video.Id,
                        assessment = 2
                    };
                    db.LikeDislikes.Add(likedislike);
                    db.SaveChanges();

                } else if(dislike.assessment == 1)
                {
                    db.LikeDislikes.Remove(dislike);
                    LikeDislike likedislike = new LikeDislike()
                    {
                        Id = dislike.Id,
                        id_user = user.Id,
                        id_video = video.Id,
                        assessment = 2
                    };
                    db.LikeDislikes.Add(likedislike);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DelFriend(int id)
        {
            string name = User.Identity.Name;
            User user = db.Users.FirstOrDefault(p => p.Email == name);
            Friend friend = db.Friends.FirstOrDefault(p => p.id_user == user.Id && p.id_user_2 == id);
            Friend friend_2 = db.Friends.FirstOrDefault(p => p.id_user_2 == user.Id && p.id_user == id);
            db.Friends.Remove(friend);
            db.Friends.Remove(friend_2);
            db.SaveChanges();
            return RedirectToAction("Profile", "Home");
        }

        public ActionResult AddFriend(int id)
        {
            string name = User.Identity.Name;
            User user = db.Users.FirstOrDefault(p => p.Email == name);
            Friend friend = new Friend()
            {
                Id = db.Friends.Count() + 1,
                id_user = user.Id,
                id_user_2 = id
            };
            db.Friends.Add(friend);
            Friend friend_2 = new Friend()
            {
                Id = db.Friends.Count() + 2,
                id_user_2 = user.Id,
                id_user = id
            };
            db.Friends.Add(friend_2);
            db.SaveChanges();
            return RedirectToAction("Profile", "Home");
        }
    }
}