using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
        public ActionResult InitData()
        {
            Init();

            return null;
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
        public ActionResult Init()
        {
            var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            string[] admin = { "admin@example.com" };
            string[] mod = { "mod@example.com" };
            string[] support = { "support@example.com" };

            const string password = "Admin@123456";

            string[] roleName = { "Admin", "Mod", "Support" };

            foreach (var r in roleName)
            {
                //Create Role Admin if it does not exist
                var role = roleManager.FindByName(r);
                if (role == null)
                {
                    role = new IdentityRole(r);
                    var roleresult = roleManager.Create(role);
                }

            }

            foreach (var a in admin)
            {
                var user = userManager.FindByName(a);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = a, Email = a };
                    var result = userManager.Create(user, password);
                    result = userManager.SetLockoutEnabled(user.Id, false);
                }

                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains(roleName[0]))
                {
                    var result = userManager.AddToRole(user.Id, roleName[0]);
                }
            }

            foreach (var m in mod)
            {
                var user = userManager.FindByName(m);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = m, Email = m };
                    var result = userManager.Create(user, password);
                    result = userManager.SetLockoutEnabled(user.Id, false);
                }

                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains(roleName[1]))
                {
                    var result = userManager.AddToRole(user.Id, roleName[1]);
                }
            }

            foreach (var s in support)
            {
                var user = userManager.FindByName(s);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = s, Email = s };
                    var result = userManager.Create(user, password);
                    result = userManager.SetLockoutEnabled(user.Id, false);
                }

                var rolesForUser = userManager.GetRoles(user.Id);
                if (!rolesForUser.Contains(roleName[2]))
                {
                    var result = userManager.AddToRole(user.Id, roleName[2]);
                }
            }

            return null;
        }
    }
}