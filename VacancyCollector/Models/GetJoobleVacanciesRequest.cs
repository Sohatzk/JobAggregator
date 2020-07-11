using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VacancyCollector.Models
{
    public class GetJoobleVacanciesRequest
    {
        [JsonProperty("keywords")]
        public string Keywords { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("radius")]
        public string Radius { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }
    }
}
