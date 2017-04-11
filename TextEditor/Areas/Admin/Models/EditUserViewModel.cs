using System.Collections.Generic;
using System.Web.Mvc;

namespace TextEditor.Areas.Admin.Models
{
    public class EditUserViewModel
    { 
        public string Id { get; set; }
        
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}