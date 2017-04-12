using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;
using TextEditor.Areas.Admin.Models;

namespace TextEditor.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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
                              Address =u.Address   ,
                              PhoneNumber=u.PhoneNumber
                          }
                        );

            return View(result.ToList());
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            return View();
        }
        public ActionResult Del(string id)
        {
            var db = new ApplicationDbContext();
            var result = db.Users.Find(id);
            db.Users.Remove(result);
            db.SaveChanges();

            return RedirectToAction("Users");
        }
        [HttpPost]
        public ActionResult Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email ,Fullname=userViewModel.FullName,Address=userViewModel.Address,PhoneNumber=userViewModel.PhoneNumber};
                var adminresult =  UserManager.Create(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result =  UserManager.AddToRoles(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList( RoleManager.Roles.ToList(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Users");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }
        public  ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user =  UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles =  UserManager.GetRoles(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName=user.Fullname,
                Address=user.Address,
                PhoneNumber=user.PhoneNumber,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user =  UserManager.FindById(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Fullname = editUser.FullName;
                user.Address = editUser.Address;
                user.PhoneNumber = editUser.PhoneNumber;

                var userRoles =  UserManager.GetRoles(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result =  UserManager.AddToRoles(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result =  UserManager.RemoveFromRoles(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Users");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }
    }
}
