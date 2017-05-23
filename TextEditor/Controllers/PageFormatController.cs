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
    public class PageFormatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PageFormat
        public ActionResult Index()
        {
            return View(db.PageFormat.ToList());
        }

        // GET: PageFormat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageFormat pageFormat = db.PageFormat.Find(id);
            if (pageFormat == null)
            {
                return HttpNotFound();
            }
            return View(pageFormat);
        }

        // GET: PageFormat/Create
        public ActionResult Create()
        {
            ViewBag.GroupFormatList = GroupFormatList();
            return View();
        }

        // POST: PageFormat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,isCentimeter,GroupId,Name,MarginLeft,MarginRight,MarginTop,MarginBottom,PaperType,FontFamily")] PageFormat pageFormat)
        {
            if (ModelState.IsValid)
            {
                db.PageFormat.Add(pageFormat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pageFormat);
        }

        // GET: PageFormat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.GroupFormatList = GroupFormatList();

            PageFormat pageFormat = db.PageFormat.Find(id);

            if (pageFormat == null)
            {
                return HttpNotFound();
            }
            return View(pageFormat);
        }

        // POST: PageFormat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,isCentimeter,GroupId,Name,MarginLeft,MarginRight,MarginTop,MarginBottom,PaperType,FontFamily")] PageFormat pageFormat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pageFormat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pageFormat);
        }

        // GET: PageFormat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PageFormat pageFormat = db.PageFormat.Find(id);
            if (pageFormat == null)
            {
                return HttpNotFound();
            }
            return View(pageFormat);
        }

        // POST: PageFormat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PageFormat pageFormat = db.PageFormat.Find(id);
            db.PageFormat.Remove(pageFormat);
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

        public List<SelectListItem> GroupFormatList()
        {
            var pageformat = from c in db.FormatGroup
                             select new SelectListItem
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             };
            List<SelectListItem> item = new List<SelectListItem>();
            item.AddRange(pageformat);
            return item;
        }
    }
}
