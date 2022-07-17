using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NPAU.Areas.Admin.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole CurrentRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CurrentRole = await _roleManager.FindByIdAsync(id);

            if (CurrentRole == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CurrentRole = await _roleManager.FindByIdAsync(id);
                    var result = await _roleManager.DeleteAsync(CurrentRole);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        return NotFound();
                    }
                    return RedirectToPage("./Index", new { message = "Role Successfully Deleted" });
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Page();
        }
    }
}