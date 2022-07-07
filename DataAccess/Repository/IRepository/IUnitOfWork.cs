﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        
        ICoursesRepository Course { get; }
        IInstructorRepository Instructor { get; }
        IGuardiansRepository Guardian { get; }
        IRatingsRepository Rating { get; }
        IStudentsRepository Student { get; }
        ITermsRepository Term { get; }
        IAssessmentsRepository Assessment { get; }
        IAttendanceRepository Attendance { get; }
        ICourseEnrollmentsRepository CourseEnrollment { get; }
        ICourseSessionsRepository CourseSession { get; }
        IDocTypeRepository DocType { get; }
        IGradeRepository Grade { get; }
        INoteTypeRepository NoteType { get; }
        ISchoolRepository School { get; }
        IStudentDocRepository StudentDoc { get; }
        IStudentNoteRepository StudentNote { get; }
        ISubjectRepository Subject { get; }
        void Save();
    }
}
