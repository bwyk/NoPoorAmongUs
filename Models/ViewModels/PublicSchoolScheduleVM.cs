﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.ViewModels
{
    public class PublicSchoolScheduleVM
    {
        public PublicSchoolSchedule PublicSchoolSchedules { get; set; }
        public Student Student { get; set; }

    }
}
