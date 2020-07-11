using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Entities
{
    public class Respond
    {
        public long RespondId { get; set; }

        public long VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    
        public long? ResumeId { get; set; }

        public AppFile Resume { get; set; }
    }
}
