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
    public class PagePropertiesFormatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PagePropertiesFormat
        public ActionResult Index()
        {
            return View(db.PagePropertiesFormat.ToList());
        }

        // GET: PagePropertiesFormat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagePropertiesFormat pagePropertiesFormat = db.PagePropertiesFormat.Find(id);
            if (pagePropertiesFormat == null)
            {
                return HttpNotFound();
            }
            return View(pagePropertiesFormat);
        }

        // GET: PagePropertiesFormat/Create
        public ActionResult Create()
        {
            ViewBag.PageFormatList = PageFormatList();
            return View();
        }

        // POST: PagePropertiesFormat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PageId,Row,Name,Size,Bold,Italic")] PagePropertiesFormat pagePropertiesFormat)
        {
            if (ModelState.IsValid)
            {
                db.PagePropertiesFormat.Add(pagePropertiesFormat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pagePropertiesFormat);
        }

        // GET: PagePropertiesFormat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagePropertiesFormat pagePropertiesFormat = db.PagePropertiesFormat.Find(id);
            if (pagePropertiesFormat == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageFormatList = PageFormatList();
            return View(pagePropertiesFormat);
        }

        // POST: PagePropertiesFormat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PageId,Row,Name,Size,Bold,Italic")] PagePropertiesFormat pagePropertiesFormat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagePropertiesFormat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pagePropertiesFormat);
        }

        // GET: PagePropertiesFormat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagePropertiesFormat pagePropertiesFormat = db.PagePropertiesFormat.Find(id);
            if (pagePropertiesFormat == null)
            {
                return HttpNotFound();
            }
            return View(pagePropertiesFormat);
        }

        // POST: PagePropertiesFormat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PagePropertiesFormat pagePropertiesFormat = db.PagePropertiesFormat.Find(id);
            db.PagePropertiesFormat.Remove(pagePropertiesFormat);
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

        public List<SelectListItem> PageFormatList()
        {
            var pageformat = from c in db.PageFormat
                             join g in db.FormatGroup
                             on c.GroupId equals g.Id
                             select new SelectListItem
                             {
                                 Text = c.Name + " - " + g.Name,
                                 Value = c.Id.ToString()
                             };

           

            List<SelectListItem> item = new List<SelectListItem>();
            item.AddRange(pageformat);
            return item;
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

        public string GetPageFormatName(int? id)
        {
            if (id == null) return "Null";
            var p = db.PageFormat.Find(id);
            if (p == null)
                return "Null";
            else
                return p.Name;
        }
    }
}
