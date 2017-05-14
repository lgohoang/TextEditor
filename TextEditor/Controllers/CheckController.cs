using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
using System.Xml.Linq;
using TextEditor.Extends;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;

namespace TextEditor.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CheckController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Extends.Library.Gramar gm = new Extends.Library.Gramar();
        new Extends.Library.File File = new Extends.Library.File();


        public ActionResult Gramar(int id)
        {
            //lấy record file trong database bằng id truyền vào
            var file = db.FileTable.Find(id);
            if (file == null) return HttpNotFound();

            //đọc file vào bộ nhớ ram
            byte[] temp = null;
            using (var fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(file.Path + "/" + file.Name), FileMode.Open, FileAccess.Read))
            {
                temp = new byte[fs.Length];
                fs.Read(temp, 0, (int)fs.Length);
            }

            MemoryStream ms = new MemoryStream();
            ms.Write(temp, 0, temp.Length);


            string pathIn = "~/Temp/" + file.Name + ".txt";
            string pathOut = "~/Temp/Tokenizer_" + file.Name + ".txt";
            string pathDict = "~/Extends/Tokenizer/Dict.txt";

            File.DocxToTxt(ms, System.Web.HttpContext.Current.Server.MapPath(pathIn));

            File.Tokenizer(System.Web.HttpContext.Current.Server.MapPath(pathIn), System.Web.HttpContext.Current.Server.MapPath(pathOut));

            string text = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(pathOut));
            IEnumerable<string> dict = System.IO.File.ReadLines(System.Web.HttpContext.Current.Server.MapPath(pathDict));

            string[] array = text.Split(' ');

            List<string> Error = new List<string>();

            foreach(var a in array)
            {
                if (!a.Equals(""))
                {
                    var s = a.Replace("_", " ");

                    var t = Regex.Replace(s, @"[0-9\-]", string.Empty);
                    var result = Regex.Replace(t, "[~!@#$%^&*()_+`\\-=.,?/'\";:\\]\\[\\–\\{\\}]", string.Empty);

                    if(result != "")
                    {
                        var sa = result.Split(' ');
                        if (sa.Length > 2)
                        {
                            foreach (var s2 in sa)
                            {
                                var i = dict.Where(x => x.Equals(s2, StringComparison.OrdinalIgnoreCase)).ToList(); ;

                                if (i.Count <= 0)
                                {
                                    Debug.WriteLine(sa + " sai chính tả");
                                    Error.Add(s2.Trim('\n'));
                                }
                            }
                        }
                        else
                        {
                            var i = dict.Where(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)).ToList(); ;

                            if (i.Count <= 0)
                            {
                                Debug.WriteLine(s + " sai chính tả");
                                Error.Add(s.Trim('\n'));
                            }
                        }
                    }
                    
                    
                }
            }


            ViewBag.Html = WordToHtml(ms);
            ViewBag.ErrorList = Error;

            return View();
        }

        


        public ActionResult Format(int id)
        {
            //lấy record file trong database bằng id truyền vào
            var file = db.FileTable.Find(id);
            if (file == null) return HttpNotFound();

            //lấy format của trang lúc upload chọn
            var pageFilter = db.PageFormat.Find(file.PageId);
            if (pageFilter == null) return HttpNotFound();

            //lay format của loại trang
            var filter = (from ppf in db.PagePropertiesFormat
                          where ppf.PageId == pageFilter.Id
                          orderby ppf.Row ascending
                          select ppf).ToList();
            if (filter == null) return HttpNotFound();

            //đọc file vào bộ nhớ ram
            byte[] temp = null;
            using (var fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(file.Path + "/" + file.Name), FileMode.Open, FileAccess.Read))
            {
                temp = new byte[fs.Length];
                fs.Read(temp, 0, (int)fs.Length);
            }

            MemoryStream ms = new MemoryStream();
            ms.Write(temp, 0, temp.Length);

            //Docx.dll (Novacode) load file từ bộ nhớ ram
            var docx = DocX.Load(ms);

            //khởi tạo 1 list đoạn văn thường mỗi dòng là một đoạn văn trừ một số trường hợp hy hữu
            var paragraphs = new List<ParagraphInfo>();
            int index = 1;

            //tìm kiếm các đoạn văn trong file docx vừa load, xong thêm vào list các đoạn văn ở trên
            foreach (var p in docx.Paragraphs)
            {
                var info = new ParagraphInfo
                {
                    Paragraph = p,
                    Row = index
                };

                //kiểm tra đoạn văn không phải khoảng trắng, xuống dòng trống thì thêm vào list
                if (!p.Text.Trim().Equals(""))
                {
                    paragraphs.Add(info);
                    index++;

                    //var f = p.MagicText[0].formatting;
                    //string font_family = f.FontFamily.Name;
                    //int size = (int)f.Size.Value;

                    //System.Drawing.Font font = new System.Drawing.Font(font_family, size);

                    //var l = LineSpacing(font, p.Text);
                }
            }

            //khởi tạo từ điển để chứa các lỗi
            var fv = new FormatView();
            fv.Page.isOk = true;

            fv.Page.MarginTop.FileValue = docx.MarginTop.ToString();
            fv.Page.MarginTop.DBValue = pageFilter.MarginTop.ToString();
            if (!(pageFilter.MarginTop == docx.MarginTop))
            {
                fv.Page.isOk = false;
                fv.Page.MarginTop.isError = true;

            }

            fv.Page.MarginBottom.FileValue = docx.MarginBottom.ToString();
            fv.Page.MarginBottom.DBValue = pageFilter.MarginBottom.ToString();
            if (!(pageFilter.MarginBottom == docx.MarginBottom))
            {
                fv.Page.isOk = false;
                fv.Page.MarginBottom.isError = true;

            }

            fv.Page.MarginLeft.FileValue = docx.MarginLeft.ToString();
            fv.Page.MarginLeft.DBValue = pageFilter.MarginLeft.ToString();
            if (!(pageFilter.MarginLeft == docx.MarginLeft))
            {
                fv.Page.isOk = false;
                fv.Page.MarginLeft.isError = true;

            }

            fv.Page.MarginRight.FileValue = docx.MarginRight.ToString();
            fv.Page.MarginRight.DBValue = pageFilter.MarginRight.ToString();
            if (!(pageFilter.MarginRight == docx.MarginRight))
            {
                fv.Page.isOk = false;
                fv.Page.MarginRight.isError = true;

            }

            var ppt = PaperSize(docx.PageWidth, docx.PageHeight);
            fv.Page.PaperType.FileValue = ppt;
            fv.Page.PaperType.DBValue = pageFilter.PaperType;
            if (!(pageFilter.PaperType == ppt))
            {
                fv.Page.isOk = false;
                fv.Page.PaperType.isError = true;
                
            }

            //duyệt list bộ lọc từ db

            foreach (var f in filter)
            {
                bool isOK = true;

                PropertiesView pv = new PropertiesView();
                //get row check
                var r = paragraphs[f.Row];

                //nếu 1 dòng có nhiều định dạng thì sai
                if (r.Paragraph.MagicText.Count > 1)
                {
                    isOK = false;
                    pv.Name += "Có quá nhiều định dạng, ";
                    
                }
                else
                {

                    var format = r.Paragraph.MagicText[0].formatting;
                    if (!(f.Size == format.Size))
                    {
                        isOK = false;
                        pv.FontSize.isError = true;
                        pv.FontSize.FileValue = format.Size.ToString();
                        pv.FontSize.DBValue = f.Size.ToString();
                        pv.Name += "Font Size không khớp, ";
                    }
                    if (!(f.Bold == format.Bold))
                    {
                        if (format.Bold == null)
                        {
                            if (f.Bold)
                            {
                                pv.FontBold.isError = true;
                                pv.FontBold.FileValue = null;
                                pv.FontBold.DBValue = f.Bold.ToString();
                                pv.Name += "Font Bold không đọc được, ";
                            }
                            else
                            {
                                pv.FontBold.isError = false;
                                pv.Name += "Font Bold không khớp, ";
                            }
                            
                        }
                        else
                        {
                            pv.FontBold.isError = true;
                            pv.FontBold.FileValue = format.Bold.ToString();
                            pv.FontBold.DBValue = f.Bold.ToString();
                            isOK = false;
                            pv.Name += "Font Bold không khớp, ";
                        }

                        pv.FontBold.isError = true;
                        pv.FontBold.FileValue = format.Bold.ToString();
                        pv.FontBold.DBValue = f.Bold.ToString();
                    }
                    if (!(f.Italic == format.Italic))
                    {
                        if (format.Italic == null)
                        {
                            if (f.Italic)
                            {
                                pv.FontItalic.isError = true;
                                pv.FontItalic.FileValue = null;
                                pv.FontItalic.DBValue = f.Italic.ToString();
                                pv.Name += "Font Italic không đọc được, ";
                            }
                            else
                            {
                                pv.FontBold.isError = false;
                            }

                        }
                        else
                        {
                            pv.FontItalic.isError = true;
                            pv.FontItalic.FileValue = format.Italic.ToString();
                            pv.FontItalic.DBValue = f.Italic.ToString();
                            isOK = false;
                            pv.Name += "Font Italic không khớp, ";
                        }

                    }
                    if(format.FontFamily == null && pageFilter.FontFamily != null)
                    {
                        isOK = false;
                    }
                    else
                    {
                        if (!pageFilter.FontFamily.Equals(format.FontFamily.Name))
                        {
                            if (format.FontFamily == null)
                            {
                                if (f.Italic)
                                {
                                    fv.Page.FontFamily.isError = true;
                                    fv.Page.FontFamily.FileValue = null;
                                    fv.Page.FontFamily.DBValue = pageFilter.FontFamily;
                                    pv.Name += "Font Family không đọc được, ";
                                }
                                else
                                {
                                    fv.Page.FontFamily.isError = false;
                                }

                            }
                            else
                            {
                                fv.Page.FontFamily.isError = true;
                                fv.Page.FontFamily.FileValue = format.FontFamily.Name;
                                fv.Page.FontFamily.DBValue = pageFilter.FontFamily;
                                isOK = false;
                                pv.Name += "Font Family không khớp, ";
                            }
                        }
                    }
                    
                }

                if (!isOK)
                {
                    fv.Properties[r.Paragraph.Text] = pv;
                }
            }

            ViewBag.Html = WordToHtml(ms);
            return View(fv);
        }

        public int LineSpacing(System.Drawing.Font f, string str)
        {
            var l = str.Length + 1024;
            Bitmap b = new Bitmap(l, l);
            Graphics g = Graphics.FromImage(b);
            SizeF s = g.MeasureString(str, f);

            return (int)s.Height;
        }

        public string PaperSize(float w, float h)
        {
            Models.Size A3 = new Models.Size { Height = 1190F, Width = 841F };
            Models.Size A4 = new Models.Size { Height = 841F, Width = 595F };

            if (w == A3.Width && h == A3.Height)
                return "A3";
            if (w == A4.Width && h == A4.Height)
                return "A4";

            return "letter";
        }


        public string WordToHtml(MemoryStream ms)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(ms, true))
            {
                HtmlConverterSettings settings = new HtmlConverterSettings(){};
                XElement html = HtmlConverter.ConvertToHtml(doc, settings);
                return html.ToStringNewLineOnAttributes();
            }
        }

        public class Error
        {
            public Error()
            {
                Format = new Dictionary<string, string>();
            }
            public string Name { get; set; }
            public Dictionary<string, string> Format { get; set; }

        }

        public class ParagraphInfo
        {
            public Novacode.Paragraph Paragraph { get; set; }
            public int Row { get; set; }
        }
    }
}