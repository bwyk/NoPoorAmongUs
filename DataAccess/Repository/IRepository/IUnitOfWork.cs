﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicantsRepository Applicant { get; }
        IClassesRepository Class { get; }
        ICoursesRepository Course { get; }
        IInstructorRepository Employee { get; }
        IGuardiansRepository Guardian { get; }
        IRatingsRepository Rating { get; }
        IStudentsRepository Student { get; }
        ITermsRepository Term { get; }
        IAssessmentsRepository Assessment { get; }
        IAttendanceRepository Attendance { get; }
        IClassEnrollmentsRepository ClassEnrollment { get; }
        IClassSessionsRepository ClassSession { get; }
        IDocTypeRepository DocType { get; }
        IGradeRepository Grade { get; }
        INoteTypeRepository NoteType { get; }
        ISchoolRepository School { get; }
        IStudentDocRepository StudentDoc { get; }
        IStudentNoteRepository StudentNote { get; }
        void Save();
    }
}