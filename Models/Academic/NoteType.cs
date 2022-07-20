﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class NoteType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        public string RoleId { get; set; }
        [ValidateNever]
        [ForeignKey("RoleId")]
        public IdentityRole Role { get; set; }
    }
}
