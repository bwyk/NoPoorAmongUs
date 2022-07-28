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


    [HttpGet]
    public ViewResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CourseSession obj)

    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CourseSession.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Course Session added successfully!";
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

        var CourseSessionFromDb = _unitOfWork.CourseSession.GetFirstOrDefault(t => t.Id == id);

        if (CourseSessionFromDb == null)
        {
            return NotFound();
        }

        return View(CourseSessionFromDb);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(CourseSession obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CourseSession.Update(obj);
            _unitOfWork.Save(); 
            TempData["success"] = "Course Session updated successfully!";
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

        _unitOfWork.CourseSession.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Course Session deleted successfully.";
        return RedirectToAction("Index");
    }

}