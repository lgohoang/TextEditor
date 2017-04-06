using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TextEditor.Controllers
{
    public class CheckController : Controller
    {
        // GET: Check
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cover()
        {
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