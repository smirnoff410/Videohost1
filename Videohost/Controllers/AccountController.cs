using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Videohost.Models;

namespace Videohost.Controllers
{
    public class AccountController : Controller
    {

        // GET: Account
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if(ModelState.IsValid)
            {
                User user = null;
                using(VideohostEntities db = new VideohostEntities())
                {
                    user = db.Users.FirstOrDefault(p => p.Email == Email && p.password == Password);
                }
                if(user != null)
                {
                    FormsAuthentication.SetAuthCookie(Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View();
        }

        public ActionResult Registration()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Registration(User users)
        {
            if(ModelState.IsValid)
            {
                User user = null;
                using (VideohostEntities db = new VideohostEntities())
                {
                    user = db.Users.FirstOrDefault(p => p.Email == users.Email);
                }
                if (user == null)
                {
                    using (VideohostEntities db = new VideohostEntities())
                    {
                        user = new User
                        {
                            Id = db.Users.Count() + 1,
                            Email = users.Email,
                            password = users.password,
                            role = 2
                        };
                        db.Users.Add(user);
                        db.AboutUsers.Add(new AboutUser
                        {
                            Id = user.Id,
                            link_photo = "https://hennesseyonline.com/wp-content/uploads/2017/01/generic-profile-avatar_352864.jpg",
                            about = "Текст обо мне"
                        });
                        db.SaveChanges();

                        user = db.Users.FirstOrDefault(p => p.Email == user.Email && p.password == user.password);
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(users.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Head = "Пользователь с таким именем уже существует";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}