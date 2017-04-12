using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    [Authorize]
    public class CheckController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Check(int id)
        {
            var file = db.FileTable.Find(id);

            var filter = (from ppf in db.PagePropertiesFormat
                          where ppf.PageId == file.PageId
                          orderby ppf.Row ascending
                          select ppf).ToList();

            

            return View();
        }

        public ActionResult SideCover()
        {
            return View();
        }

        public ActionResult Format()
        {
            return View();
        }
    }
}