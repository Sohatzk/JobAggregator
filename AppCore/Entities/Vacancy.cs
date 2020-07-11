using AppCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Entities
{
    public class Vacancy
    {
        public long VacancyId { get; set; }

        public string VendorVacancyId { get; set; }

        public string VendorName { get; set; }

        public string Name { get; set; }

        public string Employer { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public ExperienceType ExperienceType { get; set; }

        public string VendorVacancyUrl { get; set; }

        public string Area { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime PublishedAt { get; set; }

        public string Description { get; set; }
        
        public string ShortDescription { get; set; }
    }
}
