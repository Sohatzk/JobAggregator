using AppCore.Entities;
using AppCore.Enums;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacancyCollector.Models;

namespace VacancyCollector
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private const string _jooble = "ua.jooble.org";
        private const string _grc = "grc.ua";
        private readonly Dictionary<string, ExperienceType> _grcExpTypes = new Dictionary<string, ExperienceType>
        {
            {
                "noExperience", ExperienceType.NoExperience
            },
            {
                "between1And3", ExperienceType.Between1And3
            },
            {
                "between3And6", ExperienceType.Between3And6
            },
            {
                "moreThan6", ExperienceType.MoreThan6
            }
        };
        private readonly Dictionary<string, EmploymentType> _grcEmpTypes = new Dictionary<string, EmploymentType>
        {
            {
                "full", EmploymentType.Full
            },
            {
                "part", EmploymentType.Part
            },
            {
                "probation", EmploymentType.Probation
            },
            {
                "project", EmploymentType.Project
            }
        };
        private readonly Dictionary<string, EmploymentType> _joobleEmpTypes = new Dictionary<string, EmploymentType>
        {
            {
                "Повна зайнятість", EmploymentType.Full
            },
            {
                "Часткова зайнятість", EmploymentType.Part
            },
            {
                "Стажування", EmploymentType.Probation
            },
            {
                "Тимчасова зайнятість", EmploymentType.Project
            }
        };

        public TimedHostedService(ILogger<TimedHostedService> logger,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vacancy Collector Service is running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3600));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            CollectJoobleVacancies();
            CollectGRCVacancies();
        }

        public void CollectGRCVacancies()
        {
            _logger.LogInformation($"Vacancy collection from {_grc} has started");

            IServiceScope scope = null;
            HttpClient httpClient = null;
            AppDbContext dbContext = null;

            try
            {
                scope = _serviceScopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                //grc.ua
                var reqQueryParams = "host=grc.ua&locale=UA";
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://api.hh.ru");
                httpClient.DefaultRequestHeaders.Add("HH-User-Agent", "");
                //https://github.com/hhru/api/blob/master/docs/areas.md
                /*
                 *При указании параметров пагинации (page, per_page) работает ограничение: 
                 * глубина возвращаемых результатов не может быть больше 2000. 
                 * Например, возможен запрос per_page=10&page=199 (выдача с 1991 по 2000 вакансию), 
                 * но запрос с per_page=10&page=200 вернёт ошибку (выдача с 2001 до 2010 вакансию).
                 */
                var maxDepth = 2000;
                var maxCollected = 20;
                var currPage = 1;
                var perPage = 20;
                var collected = 0;

                while (collected < maxCollected
                    && currPage * perPage < maxDepth)
                {
                    var searchRequestUrl = $"vacancies?area=2164&area=2180&area=2121&area=2188&area=2126&per_page={perPage}&page={currPage}&{reqQueryParams}";
                    var searchHttpResponse = httpClient.GetAsync(searchRequestUrl).Result;
                    var json = searchHttpResponse.Content.ReadAsStringAsync().Result;
                    var searchVacanciesResponse = JsonConvert.DeserializeObject<GetGRCVacanciesResponse>(json);
                    maxDepth = searchVacanciesResponse.Found;
                    var vacanciesInDb = dbContext.Vacancies
                        .Where(v => v.VendorName == _grc)
                        .ToList()
                        .Select(v => v.VendorVacancyId)
                        .ToHashSet();
                    foreach (var vacancyResponse in searchVacanciesResponse.Items)
                    {
                        if (vacanciesInDb.Contains(vacancyResponse.Id))
                            continue;

                        var detailsRequestUrl = $"vacancies/{vacancyResponse.Id}?{reqQueryParams}";
                        var detailsHttpResponse = httpClient.GetAsync(detailsRequestUrl).Result;
                        var detailsJson = detailsHttpResponse.Content.ReadAsStringAsync().Result;
                        var detailsVacancyResponse = JsonConvert.DeserializeObject<GetGRCVacancyResponse>(detailsJson);

                        var collectedVacancy = new Vacancy
                        {
                            VendorVacancyId = vacancyResponse.Id,
                            Area = vacancyResponse.Area.Name,
                            CreatedAt = DateTime.Now,
                            PublishedAt = DateTime.Parse(vacancyResponse.PublishedAt),
                            ShortDescription = vacancyResponse.Snippet.Requirement,
                            Employer = vacancyResponse.Employer.Name,
                            Name = vacancyResponse.Name,
                            VendorVacancyUrl = vacancyResponse.AlternateUrl,
                            Description = detailsVacancyResponse.Description,
                            EmploymentType = _grcEmpTypes.ContainsKey(detailsVacancyResponse.Employment.Id)
                                ? _grcEmpTypes[detailsVacancyResponse.Employment.Id]
                                : EmploymentType.Other,
                            ExperienceType = _grcExpTypes.ContainsKey(detailsVacancyResponse.Experience.Id)
                                ? _grcExpTypes[detailsVacancyResponse.Experience.Id]
                                : ExperienceType.Other,
                            VendorName = _grc
                        };

                        dbContext.Vacancies.Add(collectedVacancy);
                        collected++;
                    }

                    currPage++;
                }

                if (collected > 0)
                    dbContext.SaveChanges();

                _logger.LogInformation($"Vacancy collection has finished, saved {collected} vacancies from {_grc}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong has happened during vacancy collection from {_grc}", ex);
            }
            finally
            {
                scope?.Dispose();
                dbContext?.Dispose();
                httpClient?.Dispose();
            }
        }

        public void CollectJoobleVacancies()
        {
            _logger.LogInformation($"Vacancy collection from {_jooble} has started");

            IServiceScope scope = null;
            HttpClient httpClient = null;
            AppDbContext dbContext = null;

            try
            {
                scope = _serviceScopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var maxDepth = 2000;
                var maxCollected = 20;
                var currPage = 1;
                var perPage = 20;
                var collected = 0;

                //https://ua.jooble.org/
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://ua.jooble.org");
                collected = 0;
                currPage = 1;
                while (collected < maxCollected
                   && currPage * perPage < maxDepth)
                {
                    var searchRequest = new GetJoobleVacanciesRequest
                    {
                        Keywords = "",
                        Location = "Ukraine",
                        Radius = "0",
                        Page = currPage.ToString(),
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(searchRequest), Encoding.UTF8, "application/json");
                    var searchHttpResponse = httpClient.PostAsync("api/52be3d7d-f2d7-4283-89b4-ade189a8901a", content).Result;
                    var json = searchHttpResponse.Content.ReadAsStringAsync().Result;
                    var searchVacanciesResponse = JsonConvert.DeserializeObject<GetJoobleVacanciesResponse>(json);
                    maxDepth = searchVacanciesResponse.TotalCount;
                    var vacanciesInDb = dbContext.Vacancies
                        .Where(v => v.VendorName == _jooble)
                        .ToList()
                        .Select(v => v.VendorVacancyId)
                        .ToHashSet();
                    foreach (var vacancyResponse in searchVacanciesResponse.Items)
                    {
                        if (vacanciesInDb.Contains(vacancyResponse.Id))
                            continue;

                        var collectedVacancy = new Vacancy
                        {
                            VendorVacancyId = vacancyResponse.Id,
                            Area = vacancyResponse.Location,
                            CreatedAt = DateTime.Now,
                            PublishedAt = DateTime.Now,
                            ShortDescription = vacancyResponse.Snippet,
                            Employer = vacancyResponse.Company,
                            Name = vacancyResponse.Title,
                            VendorVacancyUrl = vacancyResponse.Link,
                            EmploymentType = _joobleEmpTypes.ContainsKey(vacancyResponse.Type)
                                ? _joobleEmpTypes[vacancyResponse.Type]
                                : EmploymentType.Other,
                            ExperienceType = ExperienceType.Other,
                            VendorName = _jooble
                        };

                        dbContext.Vacancies.Add(collectedVacancy);

                        collected++;
                    }

                    currPage++;
                }

                if (collected > 0)
                    dbContext.SaveChanges();

                _logger.LogInformation($"Vacancy collection has finished, saved {collected} vacancies from {_jooble}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong has happened during vacancy collection from {_jooble}", ex);
            }
            finally
            {
                scope?.Dispose();
                dbContext?.Dispose();
                httpClient?.Dispose();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vacancy Collector Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
