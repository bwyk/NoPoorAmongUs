using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Academic;
using Models.ViewModels;

namespace NPAU.Areas.Admin.Controllers;
[Area("Admin")]
public class CourseSessionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseSessionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CourseSession> objSessionList = _unitOfWork.CourseSession.GetAll();
        return View(objSessionList);
    }
    public async Task<IActionResult> UpsertAsync(int? id)
    {
        CourseSessionVM courseSessionVM = new()
        {
            CourseSession = new(),
            CourseList = _unitOfWork.Course.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };

        if (id == null || id == 0)
        {
            return View(courseSessionVM);
        }
        else
        {
            courseSessionVM.CourseSession = _unitOfWork.CourseSession.GetFirstOrDefault(u => u.Id == id);
            return View(courseSessionVM);

        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpsertAsync(CourseSessionVM obj)
    {
        obj.CourseList = _unitOfWork.Course.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });
        if (ModelState.IsValid)
        {
            if (obj.CourseSession.Id == 0)
            {
                var array = obj.CourseList.ToArray();
                var name = "";
                if (int.Parse(obj.CourseSession.CourseName) != 0)
                {
                    name = array[int.Parse(obj.CourseSession.CourseName) - 1].Text;
                }
                var course = _unitOfWork.Course.GetFirstOrDefault(u => u.Name == name);
                var id = course.Id;
                obj.CourseSession.Course = course;
                obj.CourseSession.CourseId = course.Id;
                obj.CourseSession.CourseName = name;
                _unitOfWork.CourseSession.Add(obj.CourseSession);
                TempData["success"] = "Course Created Successfully";

            }
            else
            {
                _unitOfWork.CourseSession.Update(obj.CourseSession);
                TempData["success"] = "Course Updated Successfully";

            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var CourseSessionFromDb = _unitOfWork.CourseSession.GetFirstOrDefault(t => t.Id == id);

        if (CourseSessionFromDb == null)
        {
            return NotFound();
        }

        return View(CourseSessionFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.CourseSession.GetFirstOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return NotFound();
        }
        var courseEnrollments = _unitOfWork.CourseEnrollment.GetAll();
        foreach (CourseEnrollment ce in courseEnrollments)
        {
            if(ce.CourseSessionId == id)
            {
                var grade = _unitOfWork.Grade.GetAll().Where(c => ce.CourseSessionId == id);
                foreach (Grade g in grade)
                {
                    _unitOfWork.Grade.Remove(g);
                }
                _unitOfWork.CourseEnrollment.Remove(ce);
            }
        }
        _unitOfWork.CourseSession.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Course Session deleted successfully.";
        return RedirectToAction("Index");
    }

}