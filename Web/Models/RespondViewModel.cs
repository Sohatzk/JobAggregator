using AppCore.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class RespondViewModel
    {
        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public long VacancyId { get; set; }

        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public string LastName { get; set; }

        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public string Phone { get; set; }

        [Required(ErrorMessage = ValidationConstants.RequiredField)]
        public IFormFile File { get; set; }
    }
}
