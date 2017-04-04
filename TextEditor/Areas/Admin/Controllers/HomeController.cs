using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;

namespace TextEditor.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            var db = new ApplicationDbContext();
            var result = (from u in db.Users
                          select new Models.UserViewModel
                          {
                              Id=u.Id,
                              FullName = u.Fullname,
                              Email=u.Email,
                              Address =u.Address   
                          }
                        );

            return View(result.ToList());
        }
        public ActionResult Del(string id)
        {
            var db = new ApplicationDbContext();
            var result = db.Users.Find(id);
            db.Users.Remove(result);
            db.SaveChanges();

            return RedirectToAction("Users");
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}
