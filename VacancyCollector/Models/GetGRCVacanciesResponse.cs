using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VacancyCollector.Models
{
    public class GetGRCVacanciesResponse
    {
        [JsonProperty("items")]
        public GetGRCVacanciesResponseItem[] Items { get; set; }

        [JsonProperty("found")]
        public int Found { get; set; }
    }

    public class GetGRCVacanciesResponseItem 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("premium")]
        public bool Premium { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("department")]
        public object Department { get; set; }

        [JsonProperty("has_test")]
        public bool HasTest { get; set; }

        [JsonProperty("response_letter_required")]
        public bool ResponseLetterRequired { get; set; }

        [JsonProperty("area")]
        public Area Area { get; set; }

        [JsonProperty("address")]
        public object Address { get; set; }

        [JsonProperty("response_url")]
        public object ResponseUrl { get; set; }

        [JsonProperty("sort_point_distance")]
        public object SortPointDistance { get; set; }

        [JsonProperty("employer")]
        public Employer Employer { get; set; }

        [JsonProperty("published_at")]
        public string PublishedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("alternate_url")]
        public string AlternateUrl { get; set; }

        [JsonProperty("snippet")]
        public Snippet Snippet { get; set; }
    }

    public partial class Area
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Phone
    {
        [JsonProperty("comment")]
        public object Comment { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class Employer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("alternate_url")]
        public Uri AlternateUrl { get; set; }

        [JsonProperty("logo_urls")]
        public LogoUrls LogoUrls { get; set; }

        [JsonProperty("vacancies_url")]
        public Uri VacanciesUrl { get; set; }

        [JsonProperty("trusted")]
        public bool Trusted { get; set; }
    }

    public partial class LogoUrls
    {
        [JsonProperty("90")]
        public Uri The90 { get; set; }

        [JsonProperty("240")]
        public Uri The240 { get; set; }

        [JsonProperty("original")]
        public Uri Original { get; set; }
    }

 

    public partial class Snippet
    {
        [JsonProperty("requirement")]
        public string Requirement { get; set; }

        [JsonProperty("responsibility")]
        public string Responsibility { get; set; }
    }

    public partial class GetGRCVacancyResponse
    {
        [JsonProperty("experience")]
        public DictionaryType Experience { get; set; }

        [JsonProperty("employment")]
        public DictionaryType Employment { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class DictionaryType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
