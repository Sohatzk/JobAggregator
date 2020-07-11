using AppCore.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class VacancyIndexViewModel
    {

        public VacancyIndexViewModel()
        {
            Vacancies = new List<VacancyIndexItemViewModel>();
            EmploymentTypes = new Dictionary<int, string>();
            ExperienceTypes = new Dictionary<int, string>();
        }


        public List<VacancyIndexItemViewModel> Vacancies { get; set; }

        public EmploymentType? EmploymentType { get; set; }

        public ExperienceType? ExperienceType { get; set; }

        public string SearchQuery { get; set; }

        public Dictionary<int, string> EmploymentTypes { get; set; }

        public Dictionary<int, string> ExperienceTypes { get; set; }

    }
}
