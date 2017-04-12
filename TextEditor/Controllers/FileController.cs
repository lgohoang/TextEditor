using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Novacode;
using VnToolkit;
using System.Text.RegularExpressions;

namespace TextEditor.Controllers
{
    public class FileController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult CheckFormat(int id)
        {
            var file = db.FileTable.Find(id);
            if(file == null)
            {
                return HttpNotFound();
            }

            var filename = file.Path + "/" + file.Name;

            byte[] memory = null;

            using(var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                memory = new byte[fs.Length];
                fs.Read(memory, 0, (int)fs.Length);
            }

            var ms = new MemoryStream(memory);


            var dict = new Dictionary<string, int>();
            dict.Add("tentruong", 0);
            dict.Add("hoten", 1);
            dict.Add("khoa", 2);
            dict.Add("he", 3);
            dict.Add("tieude", 4);
            dict.Add("nganh", 5);
            dict.Add("detai", 6);
            dict.Add("nam", 7);

            ProcessController pc = new ProcessController();
            DocxView view = pc.GenResult(ms, dict);

            return View(view);
        }



        public ActionResult CheckGramar(int id)
        {
            var file = db.FileTable.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            var path = Server.MapPath(file.Path + "/" + file.Name);

            byte[] memory = null;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                memory = new byte[fs.Length];
                fs.Read(memory, 0, (int)fs.Length);
            }
            var ms = new MemoryStream(memory);

            var docx = DocX.Load(ms);

            foreach (var p in docx.Paragraphs)
            {
                var result = Regex.Replace(p.Text, @"[0-9\-]", string.Empty);
                var res = Regex.Replace(result, "[~!@#$%^&*()_+`\\-=.,?/'\";:\\]\\[\\–\\{\\}]", string.Empty);
                var temp = res.Split(' ');
                foreach( var t in temp)
                {
                    ViewBag.Content += "String: " + t + " is: " + Check(t) + " ";
                }
            }
            

            return View();
        }

        public bool Check(string am)
        {
            var ctam = GetAmGiua(am.ToLower());
            if(ctam == null)
            {
                return false;
            }
            var am_giua_ok = FindString(ctam.AmGiua, am_giua);
            if (!am_giua_ok)
            {
                return false;
            }
            var am_dau_ok = FindString(ctam.AmDau, am_dau);
            

            if (!am_dau_ok)
            {
                return false;
            }

            var am_cuoi_ok = FindString(ctam.AmCuoi, am_cuoi);

            if (!am_cuoi_ok)
            {
                return false;
            }

            return true;

        }

        public bool FindString(string key, string[]list)
        {
            if (key.Equals(""))
                return true;
            foreach(var l in list)
            {
                if (l.Equals(key.ToLower()))
                    return true;
            }
            return false;
        }

        public class CTAM
        {
            public string AmDau { get; set; }
            public string AmGiua { get; set; }
            public string AmCuoi { get; set; }
        }

        public CTAM GetAmGiua(string am)
        {
            string AmDau = "";
            string AmGiua = "";
            string AmCuoi = "";
            bool ok = false;
            string[] am_tiet = am.ToCharArray().Select(c => c.ToString()).ToArray();
            switch (am_tiet.Length)
            {
                case 1:
                    {
                        AmDau = "";
                        AmGiua = am;
                        AmCuoi = "";
                        ok = true;
                        break;
                    }
                case 2:
                    {
                        foreach (var ad in am_dau)
                        {
                            if (ad == am_tiet[0])
                            {
                                AmDau = ad;
                                AmGiua = am.Replace(ad, "");
                                AmCuoi = "";
                                ok = true;
                            }
                        }
                        if (!ok)
                        {
                            foreach (var ac in am_cuoi)
                            {
                                AmDau = "";
                                AmGiua = am.Replace(ac, "");
                                AmCuoi = ac;
                                ok = true;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        string tmp_am_dau = am_tiet[0] + am_tiet[1];
                        foreach (var ad in am_dau)
                        {
                            if (ad == tmp_am_dau)
                            {
                                AmDau = ad;
                                AmGiua = am.Replace(tmp_am_dau, "");
                                AmCuoi = "";
                                ok = true;
                            }
                        }
                        if (!ok)
                        {
                            tmp_am_dau = "";
                            string tmp_am_cuoi = am_tiet[2] + am_tiet[1];
                            foreach (var ac in am_cuoi)
                            {
                                if (ac == tmp_am_cuoi)
                                {
                                    AmDau = "";
                                    AmGiua = am.Replace(ac, "");
                                    AmCuoi = ac;
                                }
                            }
                        }
                        if (!ok)
                        {
                            tmp_am_dau = am_tiet[0];
                            foreach (var ad in am_dau)
                            {
                                if (ad == tmp_am_dau)
                                {
                                    AmDau = ad;
                                    AmGiua = am.Replace(ad, "");
                                    am_tiet = AmGiua.ToCharArray().Select(c => c.ToString()).ToArray();
                                    foreach (var ac in am_cuoi)
                                    {
                                        try
                                        {
                                            if (ac == am_tiet[1])
                                            {
                                                AmGiua = AmGiua.Replace(ac, "");
                                                AmCuoi = ac;
                                                ok = true;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        
                                    }
                                    if (!ok)
                                    {

                                        AmGiua = AmGiua;
                                        AmCuoi = "";
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6: { break; }
            }

            if(AmGiua == "")
            {
                return null;
            }

            var ctam = new CTAM();
            ctam.AmDau = AmDau;
            ctam.AmGiua = AmGiua;
            ctam.AmCuoi = AmCuoi;

            return ctam;

        }




        public string[] am_dau = new string[] { "b", "c", "ch", "d", "đ", "g", "gh", "h", "k", "kh", "l", "m", "n", "ng", "ngh", "nh", "p", "ph", "q", "r", "s", "t", "th", "tr", "v", "x", "none" };
        public string[] am_cuoi = new string[] { "c", "ch", "m", "n", "ng", "nh", "p", "t", "none" };
        public string[] am_giua = new string[] { "a","á","ã","à","ạ","ấ","ầ","ẫ","ậ","â","ắ","ằ","ặ","ẵ","ă","ô","ồ","ố","ộ","ỗ","u","ụ","ú","ù","ũ","u","i","ì","í","ị","ĩ","e","é","è","ẹ","ẽ","ê","ề","ế","ệ","ễ","y","ý","ỳ","ỵ","ỹ","uy","ùy","úy","ũy","ụy","ai","ái","ài","ại","ãi","ơi","ới","ời","ỡi","ợi","o","ò","ó","ọ","õ","ơ","ờ","ớ","ợ","ỡ","oc","óc","ọc","iện","iê","iề","iế","iệ","iễ","iếu","iều","iễu","iệu","iêu","an","ản","án","àn","ạn","an","iên","iện","iền","iễn","iến","uật","uất","uân","uận","uần","uấn","uận","uẫn","ư","ự","ừ","ứ","ữ","inh","ính","ình","ĩnh","ịnh" };



        // GET: File
        public ActionResult Index()
        {
            var filetable = db.FileTable.ToList();
            var model = new CrudFileTable();
            model.FiletableView = filetable;

            return View(model);
        }

        const string path = "~/Upload";

        [HttpPost]
        public ActionResult Index(CrudFileTable cft, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                var file = new FileTable();
                file.Time = DateTime.Now;
                file.Path = path;
                file.UserId = User.Identity.GetUserId();
                file.Name = file.Time.Year+file.Time.Month+file.Time.Day+file.Time.Hour+file.Time.Minute+file.Time.Second +"_"+ Path.GetFileName(fileUpload.FileName);

                if (fileUpload.ContentLength != 0)
                {
                    string pathForSaving = Server.MapPath(path);
                    if (CreateFolderIfNeeded(pathForSaving))
                    {
                        try
                        {
                            byte[] t = new byte[fileUpload.InputStream.Length];
                            fileUpload.InputStream.Read(t, 0, (int)fileUpload.InputStream.Length);
                            WriteFile(file.Path + "/" + file.Name, t);

                            db.FileTable.Add(file);
                            db.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                return RedirectToAction("index");
            }
            return View(cft);
        }

        public static void WriteFile(string fileName, byte[] bytes)
        {

            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(fileName)))
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(fileName));
            try
            {
                using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(fileName), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            return View();
        }
    }
}