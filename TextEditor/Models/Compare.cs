using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class FormatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PageFormat
    {
        public bool isCentimeter { get; set; }
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }
        public float MarginLeft { get; set; }
        public float MarginRight { get; set; }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }
        public string PaperType { get; set; }
        public string FontFamily { get; set; }
    }

    public class PagePropertiesFormat
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int Row { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
    }

    public class FormatView
    {
        public FormatView()
        {
            Page = new PageView();
            Properties = new Dictionary<string, PropertiesView>();
        }
        public PageView Page { get; set; }
        public Dictionary<string,PropertiesView> Properties { get; set; }
    }

    public class PageView
    {
        public PageView()
        {
            MarginTop = new Error();
            MarginBottom = new Error();
            MarginLeft = new Error();
            MarginRight = new Error();
            PaperType = new Error();
            FontFamily = new Error();
        }
        public bool isOk { get; set; }
        public Error MarginTop { get; set; }
        public Error MarginBottom { get; set; }
        public Error MarginLeft { get; set; }
        public Error MarginRight { get; set; }
        public Error PaperType { get; set; }
        public Error FontFamily { get; set; }
    }

    public class PropertiesView
    {
        public PropertiesView()
        {
            FontSize = new Error();
            FontBold = new Error();
            FontItalic = new Error();
        }
        public bool isOk { get; set; }
        public string Name { get; set; }
        public Error FontSize { get; set; }
        public Error FontBold { get; set; }
        public Error FontItalic { get; set; }
    }

    public class Error
    {
        public bool isError { get; set; }
        public string FileValue { get; set; }
        public string DBValue { get; set; }
    }

    public class Gramar
    {
        public Gramar()
        {
            Error = new List<string>();
        }
        public List<string> Error { get; set; }
    }
}