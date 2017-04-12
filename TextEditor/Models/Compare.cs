using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class PageFormat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }
        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
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
}