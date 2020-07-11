using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VacancyCollector.Models
{
    public class GetJoobleVacanciesResponse
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("jobs")]
        public GetJoobleVacanciesResponseItem[] Items { get; set; }
    }

    public partial class GetJoobleVacanciesResponseItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("salary")]
        public string Salary { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

}
