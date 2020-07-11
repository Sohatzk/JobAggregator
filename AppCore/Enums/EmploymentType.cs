using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppCore.Enums
{
    public enum EmploymentType
    {
        [Description("Повна зайнятість")]
        Full,
        
        [Description("Часткова зайнятість")]
        Part,

        [Description("Проектна робота")]
        Project,

        [Description("Волонтерство")]
        Volunteer,

        [Description("Стажування")]
        Probation,

        [Description("Інше")]
        Other
    }
}
