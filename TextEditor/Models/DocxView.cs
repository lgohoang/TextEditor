using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class DocxView
    {
        public Margin Margin { set; get; }
        public PageLayout PageLayout { get; set; }
    }

    public class Margin
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

    }

    public class PageLayout
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }

    public class PaperSize
    {
        PageLayout A4 = new PageLayout { Height = 29.7, Width = 21 };
        PageLayout A3 = new PageLayout { Height = 42, Width = 29.7 };

        public string PaperType(PageLayout pl)
        {
            if (pl.Height == A4.Height && pl.Width == A4.Width)
                return "A4";
            if (pl.Height == A3.Height && pl.Width == A3.Width)
                return "A3";
            return "letter";
        }
    }
}