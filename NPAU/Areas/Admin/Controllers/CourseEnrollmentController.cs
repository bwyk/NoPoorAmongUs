using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Academic;
using Models.ViewModels;

namespace NPAU.Areas.Admin.Controllers;
[Area("Admin")]
public class CourseEnrollmentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseEnrollmentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CourseEnrollment> objCourseEnrollmentList = _unitOfWork.CourseEnrollment.GetAll();
        return View(objCourseEnrollmentList);
    }


    [HttpGet]
    public ViewResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CourseEnrollment obj)

    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CourseEnrollment.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Course Enrollment added successfully!";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var CourseEnrollmentFromDb = _unitOfWork.CourseEnrollment.GetFirstOrDefault(t => t.Id == id);

        if (CourseEnrollmentFromDb == null)
        {
            return NotFound();
        }

        return View(CourseEnrollmentFromDb);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(CourseEnrollment obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CourseEnrollment.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Course Enrollment updated successfully!";
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

        var CourseEnrollmentFromDb = _unitOfWork.CourseEnrollment.GetFirstOrDefault(t => t.Id == id);

        if (CourseEnrollmentFromDb == null)
        {
            return NotFound();
        }

        return View(CourseEnrollmentFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.CourseEnrollment.GetFirstOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.CourseEnrollment.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Course Enrollment deleted successfully.";
        return RedirectToAction("Index");
    }

}