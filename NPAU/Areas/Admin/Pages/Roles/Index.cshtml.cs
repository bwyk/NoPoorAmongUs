using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Repository.IRepository;

namespace NPAU.Areas.Admin.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        [BindProperty]
        public IdentityRole SelectedRole { get; set; }

        public IndexModel(RoleManager<IdentityRole> roleManager)
        {

            _roleManager = roleManager;
        }

        public string Message { get; set; }
        public List<IdentityRole> AllRoles { get; set; }

        public void OnGet(string message = "")
        {
            Message = message;
            AllRoles = _roleManager.Roles.ToList();
        }
    }
}
