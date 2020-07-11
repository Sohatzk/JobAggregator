using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class VacancyIndexItemViewModel
    {
        public long VacancyId { get; set; }

        public string VendorVacancyId { get; set; }

        public string VendorName { get; set; }

        public string Name { get; set; }

        public string Employer { get; set; }

        public string EmploymentType { get; set; }

        public string ExperienceType { get; set; }

        public string VendorVacancyUrl { get; set; }

        public string Area { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime PublishedAt { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }
    }
}
