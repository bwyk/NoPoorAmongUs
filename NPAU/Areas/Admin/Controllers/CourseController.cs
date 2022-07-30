using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Utilities;

namespace NPAU.Areas.Admin.Controllers;
[Area("Admin")]
public class CourseController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public CourseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> UpsertAsync(int? id)
    {
        var instructors = await _userManager.GetUsersInRoleAsync(SD.Role_Instructor);

        CourseVM courseVM = new()
        {
            Course = new(),
            SchoolList = _unitOfWork.School.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            
            InstructorList = _unitOfWork.ApplicationUser.GetAll(u => instructors.Select(i => i.Id).Contains(u.Id)).Select(i => new SelectListItem
            {
                Text = i.FirstName + ' ' + i.LastName,
                Value = i.Id.ToString()
            }),
            SubjectList = _unitOfWork.Subject.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            TermList = _unitOfWork.Term.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };

        if (id == null || id == 0)
        {
            return View(courseVM);
        }
        else
        {
            courseVM.Course = _unitOfWork.Course.GetFirstOrDefault(u => u.Id == id);
            return View(courseVM);

        }

        return View(courseVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpsertAsync(CourseVM obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.Course.Id == 0)
            {
                _unitOfWork.Course.Add(obj.Course);
                TempData["success"] = "Course Created Successfully";

            }
            else
            {
                _unitOfWork.Course.Update(obj.Course);
                TempData["success"] = "Course Updated Successfully";

            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        obj.SchoolList = _unitOfWork.School.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });
        var instructors = await _userManager.GetUsersInRoleAsync(SD.Role_Instructor);


        obj.InstructorList = _unitOfWork.ApplicationUser.GetAll(u => instructors.Select(i => i.Id).Contains(u.Id)).Select(i => new SelectListItem
        {
            Text = i.FirstName + ' ' + i.LastName,
            Value = i.Id.ToString()
        });

        obj.SubjectList = _unitOfWork.Subject.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });

        obj.TermList = _unitOfWork.Term.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });

        return View(obj);
    }

    #region API CALLS 
    [HttpGet]
    public IActionResult GetAll()
    {
        var courseList = _unitOfWork.Course.GetAll(includeProperties: "School,ApplicationUser,Subject,Term");
        return Json(new { data = courseList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Course.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error While Deleting" });
        }

        _unitOfWork.Course.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Course Deleted Successfully" });

    }

    #endregion

}
