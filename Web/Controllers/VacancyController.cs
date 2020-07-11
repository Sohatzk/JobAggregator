using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.Enums;
using AppCore.Extensions;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public VacancyController(AppDbContext appDbContext,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchQuery = null,
            EmploymentType? employmentType = null,
            ExperienceType? experienceType = null)
        {
            var vacancies = await SearchVacancies(searchQuery, employmentType, experienceType);

            var model = new VacancyIndexViewModel
            {
                Vacancies = _mapper.Map<List<VacancyIndexItemViewModel>>(vacancies.ToList()),
                SearchQuery = searchQuery,
                EmploymentType = employmentType,
                ExperienceType = experienceType
            };
            PrepareIndexModel(model);
            return View(model);
        }

        private async Task<List<Vacancy>> SearchVacancies(string searchQuery = null,
            EmploymentType? employmentType = null,
            ExperienceType? experienceType = null)
        {
            var vacancies = _appDbContext.Vacancies.AsNoTracking();

            if (!string.IsNullOrEmpty(searchQuery))
                vacancies = vacancies.Where(v => v.Name.Contains(searchQuery)
                    || v.Description.Contains(searchQuery)
                    || v.Area.Contains(searchQuery));

            if (employmentType.HasValue)
                vacancies = vacancies.Where(v => v.EmploymentType == employmentType.Value);

            if (experienceType.HasValue)
                vacancies = vacancies.Where(v => v.ExperienceType == experienceType.Value);

            vacancies = vacancies.OrderByDescending(v => v.CreatedAt);

            return await vacancies.ToListAsync();
        }

        private void PrepareIndexModel(VacancyIndexViewModel model)
        {
            model.EmploymentTypes = new Dictionary<int, string>();
            foreach (EmploymentType value in Enum.GetValues(typeof(EmploymentType)))
            {
                model.EmploymentTypes.Add((int)value, value.GetDescription());
            }

            model.ExperienceTypes = new Dictionary<int, string>();
            foreach (ExperienceType value in Enum.GetValues(typeof(ExperienceType)))
            {
                model.ExperienceTypes.Add((int)value, value.GetDescription());
            }
        }

        public async Task<IActionResult> Details(long id, bool isFromRespond = false)
        {
            var vacancy = _appDbContext.Vacancies.Find(id);
            var model = _mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.IsFromRespond = isFromRespond;
            return View(model);
        }
        
        public async Task<IActionResult> Respond(int vacancyId)
        {
            var model = new RespondViewModel { VacancyId = vacancyId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Respond(RespondViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = new AppFile
                {
                    Content = GetFileContent(model.File),
                };
                await _appDbContext.AppFiles.AddAsync(file);

                var respond = _mapper.Map<Respond>(model);
                respond.Resume = file;
                await _appDbContext.Responds.AddAsync(respond);

                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Details", new { id = model.VacancyId, isFromRespond = true });
            }

            return View(model);
        }

        private byte[] GetFileContent(IFormFile file)
        {
            byte[] content = null;
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                content = binaryReader.ReadBytes((int)file.Length);
            }
            return content;
        }
    }
}