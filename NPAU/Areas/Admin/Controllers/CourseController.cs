using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ViewResult Index()
        {
            IEnumerable<Course> objCourseList = _unitOfWork.Course.GetAll();
            return View(objCourseList);
        }
    }
}