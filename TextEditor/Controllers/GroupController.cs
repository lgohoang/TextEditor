using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    [Authorize(Roles = "Admin, Support")]
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Group
        public ActionResult Index()
        {
            return View(db.FormatGroup.ToList());
        }

        // GET: Group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormatGroup formatGroup = db.FormatGroup.Find(id);
            if (formatGroup == null)
            {
                return HttpNotFound();
            }
            return View(formatGroup);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] FormatGroup formatGroup)
        {
            if (ModelState.IsValid)
            {
                db.FormatGroup.Add(formatGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formatGroup);
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormatGroup formatGroup = db.FormatGroup.Find(id);
            if (formatGroup == null)
            {
                return HttpNotFound();
            }
            return View(formatGroup);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] FormatGroup formatGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formatGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formatGroup);
        }

        // GET: Group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormatGroup formatGroup = db.FormatGroup.Find(id);
            if (formatGroup == null)
            {
                return HttpNotFound();
            }
            return View(formatGroup);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormatGroup formatGroup = db.FormatGroup.Find(id);
            db.FormatGroup.Remove(formatGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
