using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace NPAU.Areas.Admin.Pages.Roles
{
    public class UpdateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
         public UpdateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole CurrentRole { get; set; }
        [BindProperty]
        public bool IsUpdate { get; set; }

        public async Task OnGetAsync(string? id)
        {
            if (id != null)
            {
                CurrentRole = await _roleManager.FindByIdAsync(id);
                IsUpdate = true;
            }
            else
            {
                CurrentRole = new IdentityRole();
                IsUpdate = false;
            }
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!IsUpdate)
                    {
                        CurrentRole.NormalizedName = CurrentRole.Name.ToUpper();

                        await _roleManager.CreateAsync(CurrentRole);
                        return RedirectToPage("./Index", new { success = true, message = "Role Successfully Added" });
                    }
                    else
                    {
                        if(CurrentRole.NormalizedName != null)
                        {
                            CurrentRole.NormalizedName = CurrentRole.Name.ToUpper();
                            var result = await _roleManager.UpdateAsync(CurrentRole);
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First().ToString());
                                return NotFound();
                            }
                            return RedirectToPage("./Index", new { message = "Role Successfully Updated" });
                        }
                        else
                        {
                            return RedirectToPage("./Index", new { message = "Role Entry Not Changed" });
                        }
                    }
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