using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class DocxView
    {
        public DocxView()
        {
            Page = new Page();
            Cover = new Page();
        }
        public Page Page { get; set; }
        public Page Cover { get; set; }
    }

    public class Page
    {
        public Page()
        {
            Layout = new Layout();
            Font = new Font();
        }
        public Layout Layout { get; set; }
        public Font Font { get; set; }
    }

    public class Font
    {
        public float Size { get; set; }
        public string Family { get; set; }
    }

    public class Margin
    {
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }

    }

    public class Layout
    {
        public Layout()
        {
            Size = new Size();
            Margin = new Margin();
        }
        public Size Size { get; set; }
        public Margin Margin { get; set; }
    }

    public class Size
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public string PaperType { get; set; }
    }

    public class PaperSize
    {
        Size A3 = new Size { Height = 1190F, Width = 841F };
        Size A4 = new Size { Height = 841F, Width = 595F };

        public string PaperType(Size pl)
        {
            if (pl.Height == A4.Height && pl.Width == A4.Width)
                return "A4";
            if (pl.Height == A3.Height && pl.Width == A3.Width)
                return "A3";
            return "letter";
        }
    }
}