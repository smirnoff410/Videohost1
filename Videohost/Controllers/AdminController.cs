using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Videohost.Models;

namespace Videohost.Controllers
{
    public class AdminController : Controller
    {
        VideohostEntities db = new VideohostEntities();
        // GET: Admin
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {

            return View();
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "News")]
        public ActionResult Admin(News news)
        {
            Game game = db.Games.FirstOrDefault(p => p.name == news.Game.name);
            News newss = new News()
            {
                Id = db.News.Where(p => p.Id > 1).OrderByDescending(p => p.Id).FirstOrDefault().Id + 1,
                title = news.title,
                full_news = news.full_news,
                id_game = game.Id
            };
            db.News.Add(newss);
            db.SaveChanges();
            return View();
        }


        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Tournaments")]
        public ActionResult Admin(Tournament tournament)
        {
            Game game = db.Games.FirstOrDefault(p => p.name == tournament.Game.name);
            Tournament tournaments = new Tournament()
            {
                Id = db.Tournaments.Where(p => p.Id > 1).OrderByDescending(p => p.Id).FirstOrDefault().Id + 1,
                date = tournament.date,
                organizer = tournament.organizer,
                prize_fund = tournament.prize_fund,
                status = tournament.status,
                id_game = game.Id
            };
            db.Tournaments.Add(tournaments);
            db.SaveChanges();
            return View();
        }
        
        [Authorize(Roles ="admin")]
        public ActionResult DelTour(int id)
        {
            Tournament tournament = db.Tournaments.FirstOrDefault(p => p.Id == id);
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="admin")]
        public ActionResult DelNews(int id)
        {
            News news = db.News.FirstOrDefault(p => p.Id == id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="admin")]
        public ActionResult EditNews(int id, string title, string full_news)
        {
            News news = db.News.FirstOrDefault(p => p.Id == id);
            db.News.Remove(news);
            db.SaveChanges();
            news.title = title;
            news.full_news = full_news;
            db.News.Add(news);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="admin")]
        public ActionResult DelMessage(int id)
        {
            Message message = db.Messages.FirstOrDefault(p => p.Id == id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Profile", "Home");
        }
    }
}