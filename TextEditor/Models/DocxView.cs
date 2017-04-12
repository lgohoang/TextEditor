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

    public class Paragraph
    {
        public Paragraph()
        {
            MagicText = new Dictionary<string, MagicText>();
        }
        public string Text { get; set; }
        public string Alignment { get; set; }

        public Dictionary<string, MagicText> MagicText { get; set; }
    }

    public class MagicText
    {
        public MagicText()
        {
            Font = new Font();
        }
        public Font Font { get; set; }
    }


    public class Page
    {
        public Page()
        {
            Layout = new Layout();
            Paragraphs = new Dictionary<string, Paragraph>();
        }
        public Layout Layout { get; set; }
        public Dictionary<string, Paragraph> Paragraphs { get; set; }
    }

    public class Font
    {
        public float Size { get; set; }
        public string Family { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
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

    public class FormatView
    {
        public bool isOk { get; set; }
    }
}