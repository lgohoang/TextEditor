using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}