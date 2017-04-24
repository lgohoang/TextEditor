using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Novacode;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace TextEditor.Extends.Library
{
    public class File
    {
        public void DocxToTxt(MemoryStream ms, string path)
        {
            //Docx.dll (Novacode) load file từ bộ nhớ ram
            var docx = DocX.Load(ms);

            string text = "";
            //tìm kiếm các đoạn văn trong file docx vừa load, xong thêm vào list các đoạn văn ở trên
            foreach (var p in docx.Paragraphs)
            {
                //kiểm tra đoạn văn không phải khoảng trắng, xuống dòng trống thì thêm vào list
                if (!p.Text.Trim().Equals(""))
                {
                    text += p.Text + " ";
                }
            }

            using (FileStream f = System.IO.File.Open(path,FileMode.Create, FileAccess.Write))
            {
                Encoding encoding = Encoding.UTF8; //Or any other Encoding
                using (var s = new StreamWriter(f, encoding))
                {
                    s.Write(text);
                }
            }
        }


        public void Tokenizer(string input, string output)
        {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/Extends/Tokenizer/vnTokenizer.bat");
            process.StartInfo.Arguments = "-i " + input + " -o "+ output;
            process.Start();
            process.WaitForExit();
        }
    }
}