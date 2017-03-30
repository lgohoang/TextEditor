using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Extends.Library
{
    public class Convert
    {
        const float PaperClips = 28.30F; //centimeters
        public float PaperClipsToCentimeters(float ppc)
        {
            return ppc / PaperClips;
        }

        public float CentimetersToPaperClips(float centimeter)
        {
            return centimeter * PaperClips;
        }


        public double CentimeterToPixel(float centimeter, float dpi)
        {
            return (centimeter * dpi) / 2.54;
        }

        public double PixelToCentimeter(float pixel, float dpi)
        {
            return (pixel * 2.54) / dpi;
        }

        public double Round(double value, int digits)
        {
            if (digits >= 0) return Math.Round(value, digits);

            double n = Math.Pow(10, -digits);
            return Math.Round(value / n, 0) * n;
        }

        public decimal Round(decimal d, int decimals)
        {
            if (decimals >= 0) return decimal.Round(d, decimals);

            decimal n = (decimal)Math.Pow(10, -decimals);
            return decimal.Round(d / n, 0) * n;
        }
    }
}