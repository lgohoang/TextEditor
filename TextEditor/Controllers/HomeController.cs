using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Del(int id)
        {
            var db = new ApplicationDbContext();

            var result = db.Users.Find(id);

            db.Users.Remove(result);
            db.SaveChanges();
            return View(result);
        }

        public ActionResult Edit(int id)
        {
            var db = new ApplicationDbContext();

            var result = db.Users.Find(id);
            return View(result);
        }

        public ActionResult Edit(ApplicationUser user)
        {
            var db = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("Index");
            }
            return View(user);
        }

        public ActionResult Create()
        {
            var db = new ApplicationDbContext();
            return View();
        }

        public ActionResult Create(ApplicationUser user)
        {
            var db = new ApplicationDbContext();
            db.Users.Add(user);
            db.SaveChanges();
            return Redirect("index");
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
       
           
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}