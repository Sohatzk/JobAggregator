using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppCore.Enums
{
    public enum ExperienceType
    {
        [Description("Немає досвіду")]
        NoExperience,
        
        [Description("Від 1 року до 3 років")]
        Between1And3,

        [Description("Від 3 до 6 років")]
        Between3And6,

        [Description("Понад 6 років")]
        MoreThan6,

        [Description("Інше")]
        Other
    }
}
