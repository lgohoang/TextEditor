using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class CompareSample
    {
        public int Id { get; set; }
        public int PageMarginLeft { get; set; }
        public int PageMarginRight { get; set; }
        public int PageMarginTop { get; set; }
        public int PageMarginBottom { get; set; }
        public string PagePaperType { get; set; }
        public string PageFontFamily { get; set; }
        public int CoverMarginLeft { get; set; }
        public int CoverMarginRight { get; set; }
        public int CoverMarginTopt { get; set; }
        public int CoverMarginBottom { get; set; }
        public string CoverFontSize { get; set; }
        public string CoverFontFamily { get; set; }

    }
}