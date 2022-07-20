using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Guardian
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                if (MiddleName != null)
                    return FirstName + " " + MiddleName + " " + LastName;
                else
                    return FirstName + " " + LastName;
            }

        }
        [NotMapped]
        public string? Relationship { get; set; }
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
