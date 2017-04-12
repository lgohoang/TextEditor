using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Models
{
    public class FileTable
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int PageId { get; set; }
        public DateTime Time { get; set; }
    }

    public class FileTableUpload
    {
        public string Name { get; set; }
        public int PageId { get; set; }
    }
    public class CrudFileTable
    {
        public List<FileTable> FiletableView { get; set; }
        public FileTableUpload FileTableUpload { get; set; }
    }
}