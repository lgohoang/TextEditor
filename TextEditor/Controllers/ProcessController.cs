﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Novacode;
//using Novacode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using TextEditor.Extends.Library;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    [RoutePrefix("api/process")]
    public class ProcessController : ApiController
    {
        //[HttpPost]
        //[Route("upload")]
        //public async Task<HttpResponseMessage> UploadFile()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
        //    }

        //    try
        //    {
        //        var provider = new MultipartMemoryStreamProvider();
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        foreach (var file in provider.Contents)
        //        {
        //            var dataStream = await file.ReadAsByteArrayAsync();
        //            // use the data stream to persist the data to the server (file system etc)
        //            Stream stream = new MemoryStream(dataStream);

        //            using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
        //            {
        //                var convert = new Extends.Library.Convert();
        //                var view = new DocxView();
        //                var docPart = doc.MainDocumentPart;

        //                var prgph = new List<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();

        //                // Open the file read-only since we don't need to change it.

        //                prgph = doc.MainDocumentPart.Document.Body
        //                    .OfType<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
        //                    .Where(p => p.ParagraphProperties != null &&
        //                                p.ParagraphProperties.ParagraphStyleId != null &&
        //                                p.ParagraphProperties.ParagraphStyleId.Val.Value.Contains("Heading1")).ToList();

        //                var paragraphInfos = new List<ParagraphInfo>();

        //                var paragraphs = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();

        //                int pageCount =int.Parse(doc.ExtendedFilePropertiesPart.Properties.Pages.Text);

        //                var m = doc.MainDocumentPart.Document.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.PageMargin>();




        //                int pageIdx = 1;
        //                foreach (var paragraph in paragraphs)
        //                {
        //                    var run = paragraph.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Run>();

        //                    if (run != null)
        //                    {
        //                        var lastRenderedPageBreak = run.GetFirstChild<LastRenderedPageBreak>();
        //                        var pageBreak = run.GetFirstChild<Break>();
        //                        var bookmarkStart = run.GetFirstChild<BookmarkStart>();


        //                        if (lastRenderedPageBreak != null || pageBreak != null)
        //                        {
        //                            pageIdx++;
        //                        }
        //                    }

        //                    var info = new ParagraphInfo
        //                    {
        //                        Paragraph = paragraph,
        //                        PageNumber = pageIdx
        //                    };
        //                    paragraphInfos.Add(info);

        //                }


        //                foreach (var info in paragraphInfos)
        //                {
        //                    var attri = info.Paragraph;
        //                    var font = GetRunPropertyFromParagraph(info.Paragraph);


        //                    Debug.WriteLine("Page {0}/{1} - {2} : '{3}'", info.PageNumber, pageIdx, (int.Parse(font.FontSize.Val) / 2), attri.InnerText);
        //                }







        //                MemoryStream ms = new MemoryStream();
        //                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DocxView));
        //                ser.WriteObject(ms, view);
        //                string json = Encoding.Default.GetString(ms.ToArray());
        //                var response = Request.CreateResponse(HttpStatusCode.OK);
        //                response.Content = new StringContent(json, Encoding.UTF8, "text/plain");
        //                response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
        //                return response;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
        //    }
        //    return null;
        //}

        public class ParagraphInfo
        {
            public DocumentFormat.OpenXml.Wordprocessing.Paragraph Paragraph { get; set; }
            public int PageNumber { get; set; }
        }

        [HttpPost]
        [Route("cover")]
        public async Task<HttpResponseMessage> Cover()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsByteArrayAsync();
                    // use the data stream to persist the data to the server (file system etc)
                    Stream stream = new MemoryStream(dataStream);

                    var dict = new Dictionary<string, int>();
                    dict.Add("tentruong", 0);
                    dict.Add("hoten", 1);
                    dict.Add("khoa", 2);
                    dict.Add("he", 3);
                    dict.Add("tieude", 4);
                    dict.Add("nganh", 5);
                    dict.Add("detai", 6);
                    dict.Add("nam", 7);

                    MemoryStream ms = new MemoryStream();
                    DocxView view = GenResult(stream, dict);




                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DocxView));
                    ser.WriteObject(ms, view);
                    string json = Encoding.Default.GetString(ms.ToArray());
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(json, Encoding.UTF8, "text/plain");
                    response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                    return response;

                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            return null;
        }

        [HttpPost]
        [Route("sidecover")]
        public async Task<HttpResponseMessage> SideCover()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsByteArrayAsync();
                    // use the data stream to persist the data to the server (file system etc)
                    Stream stream = new MemoryStream(dataStream);

                    var dict = new Dictionary<string, int>();
                    dict.Add("tentruong", 0);
                    dict.Add("hoten", 1);
                    dict.Add("khoa", 2);
                    dict.Add("he", 3);
                    dict.Add("tieude", 4);
                    dict.Add("nganh", 5);
                    dict.Add("maso", 6);
                    dict.Add("detai", 7);
                    dict.Add("gvhd", 8);
                    dict.Add("nam", 9);

                    MemoryStream ms = new MemoryStream();
                    DocxView view = GenResult(stream, dict);




                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DocxView));
                    ser.WriteObject(ms, view);
                    string json = Encoding.Default.GetString(ms.ToArray());
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(json, Encoding.UTF8, "text/plain");
                    response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                    return response;

                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            return null;
        }

        public DocxView GenResult(Stream stream, Dictionary<string,int> col)
        {
            var view = new DocxView();
            var docx = DocX.Load(stream);
            WordprocessingDocument doc = WordprocessingDocument.Open(stream, false);
            var convert = new Extends.Library.Convert();
            var ml = convert.PaperClipsToCentimeters(docx.MarginLeft);
            var paragraphInfos = new List<ParagraphInfo>();
            view.Page.Layout.Margin.Top = docx.MarginTop;
            view.Page.Layout.Margin.Bottom = docx.MarginBottom;
            view.Page.Layout.Margin.Left = docx.MarginLeft;
            view.Page.Layout.Margin.Right = docx.MarginRight;
            view.Page.Layout.Size.Width = docx.PageWidth;
            view.Page.Layout.Size.Height = docx.PageHeight;
            var pps = new Models.PaperSize();
            view.Page.Layout.Size.PaperType = pps.PaperType(view.Page.Layout.Size);

            var paragraphs = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            int pageIdx = 1;
            foreach (var paragraph in paragraphs)
            {
                var run = paragraph.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Run>();
                var info = new ParagraphInfo
                {
                    Paragraph = paragraph,
                    PageNumber = pageIdx
                };

                if (!paragraph.InnerText.Trim().Equals(""))
                {
                    paragraphInfos.Add(info);
                }


                if (run != null)
                {
                    var lastRenderedPageBreak = run.GetFirstChild<LastRenderedPageBreak>();
                    var pageBreak = run.GetFirstChild<Break>();


                    if (lastRenderedPageBreak != null || pageBreak != null)
                    {
                        pageIdx++;
                    }
                }



            }

            foreach(var c in col)
            {
                view.Cover.Paragraphs[c.Key] = GetParagraph(paragraphInfos, docx, c.Value);
            }

            return view;
            //view.Cover.Paragraphs["tentruong"] = GetParagraph(paragraphInfos, docx, 0);
            //view.Cover.Paragraphs["hoten"] = GetParagraph(paragraphInfos, docx, 1);
            //view.Cover.Paragraphs["khoa"] = GetParagraph(paragraphInfos, docx, 2);
            //view.Cover.Paragraphs["he"] = GetParagraph(paragraphInfos, docx, 3);
            //view.Cover.Paragraphs["tieude"] = GetParagraph(paragraphInfos, docx, 4);
            //view.Cover.Paragraphs["nganh"] = GetParagraph(paragraphInfos, docx, 5);
            //view.Cover.Paragraphs["detai"] = GetParagraph(paragraphInfos, docx, 6);
            //view.Cover.Paragraphs["nam"] = GetParagraph(paragraphInfos, docx, 7);
        }

        public Models.Paragraph GetParagraph(List<ParagraphInfo> pinfo, DocX docx, int index)
        {
            var p = pinfo[index].Paragraph;

            var p2 = Paragraph(docx, p.InnerText);
            if (p2 != null)
            {
                return Views(p2, p);
            }
            return null;

        }

        public Novacode.Paragraph Paragraph(DocX docx, string text)
        {
            foreach (var p in docx.Paragraphs)
            {
                if (p.Text.Equals(text))
                {
                    return p;
                }
            }
            return null;
        }

        public Models.Paragraph Views(Novacode.Paragraph p, DocumentFormat.OpenXml.Wordprocessing.Paragraph p2)
        {
            var par = new Models.Paragraph();

            par.Alignment = p.Alignment.ToString();
            par.Text = p.Text;

            foreach (var m in p.MagicText)
            {


                var mt = new MagicText();

                if (m.formatting.FontFamily != null)
                {
                    mt.Font.Family = m.formatting.FontFamily.Name;
                }
                if (m.formatting.Size != null)
                {
                    mt.Font.Size = (float)m.formatting.Size;
                }
                if (m.formatting.Bold != null)
                {
                    mt.Font.Bold = m.formatting.Bold.Value;
                }
                if (m.formatting.Italic != null)
                {
                    mt.Font.Italic = m.formatting.Italic.Value;
                }

                par.MagicText[m.text] = mt;
            }

            return par;
        }
    }
}
