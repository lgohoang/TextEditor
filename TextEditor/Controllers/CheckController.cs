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

namespace TextEditor.Controllers
{
    [Authorize]
    public class CheckController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
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
                }
            }

            //khởi tạo từ điển để chứa các lỗi
            var Error = new List<string>();

            bool pageIsOK = true;
            string Err = ""; 

            if (!(pageFilter.MarginTop == docx.MarginTop))
            {
                Err += "MarginTop không khớp database là: "+pageFilter.MarginTop+" - file là: " + docx.MarginTop + "\n";

                pageIsOK = false;
            }
            if (!(pageFilter.MarginBottom == docx.MarginBottom))
            {
                Err += "MarginBottom không khớp database là: " + pageFilter.MarginBottom + " - file là: " + docx.MarginBottom + "\n";
                pageIsOK = false;
            }
            if (!(pageFilter.MarginLeft == docx.MarginLeft))
            {
                Err += "MarginLeft không khớp database là: " + pageFilter.MarginLeft + " - file là: " + docx.MarginLeft + "\n";
                pageIsOK = false;
            }
            if (!(pageFilter.MarginRight == docx.MarginRight))
            {
                Err += "MarginRight không khớp database là: " + pageFilter.MarginRight + " - file là: " + docx.MarginRight + "\n";
                pageIsOK = false;
            }

            if (!(pageFilter.PaperType == PaperSize(docx.PageWidth, docx.PageHeight)))
            {
                Err += "PaperType không khớp database là: " + pageFilter.PaperType + " - file là: " + PaperSize(docx.PageWidth, docx.PageHeight) + "\n";
                pageIsOK = false;
            }

            ViewBag.Height = docx.PageHeight;
            ViewBag.Width = docx.PageWidth;
            ViewBag.Paper = PaperSize(docx.PageWidth, docx.PageHeight);
            //duyệt list bộ lọc từ db

            foreach (var f in filter)
            {
                bool isOK = true;
                //get row check
                var r = paragraphs[f.Row];

                //nếu 1 dòng có nhiều định dạng thì sai
                if (r.Paragraph.MagicText.Count > 1)
                {
                    Err += "Dòng văn bản \"" + r.Paragraph.Text + "\" có quá nhiều định dạng \n";
                    Error.Add(r.Paragraph.Text);
                    isOK = false;

                }
                else
                {



                    var format = r.Paragraph.MagicText[0].formatting;
                    if (!(f.Size == format.Size))
                    {
                        Err += "Dòng văn bản \"" + r.Paragraph.Text + "\" Size không khớp database là: " + f.Size + " - file là: " + format.Size + "\n";
                        isOK = false;
                    }
                    if (!(f.Bold == format.Bold))
                    {
                        Err += "Dòng văn bản \"" + r.Paragraph.Text + "\" Bold không khớp database là: " + f.Bold + " - file là: " + format.Bold + "\n";
                        isOK = false;
                    }
                    if (!(f.Italic == format.Italic))
                    {
                        if (format.Italic == null)
                        {

                        }
                        else
                        {
                            Err += "Dòng văn bản \"" + r.Paragraph.Text + "\" Italic không khớp database là: " + f.Italic + " - file là: " + format.Italic + "\n";
                            isOK = false;
                        }

                    }
                    if (!pageFilter.FontFamily.Equals(format.FontFamily.Name))
                    {
                        Err += "FontFamily không khớp database là: " + pageFilter.FontFamily + " - file là: " + format.FontFamily.Name + "\n";
                        isOK = false;
                    }
                }

                if (!isOK)
                {
                    Error.Add(r.Paragraph.Text);
                }
            }
            var fv = new FormatView();
            fv.isOk = pageIsOK;
            ViewBag.Error = Error;
            ViewBag.Html = WordToHtml(ms);
            return View(fv);
        }

        public string PaperSize(float w, float h)
        {
            Size A3 = new Size { Height = 1190F, Width = 841F };
            Size A4 = new Size { Height = 841F, Width = 595F };

            if (w == A3.Width && h == A3.Height)
                return "A3";
            if (w == A4.Width && h == A4.Height)
                return "AA";

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