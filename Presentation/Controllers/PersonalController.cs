using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PersonalServices;

namespace Presentation.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IReadPersonalService _readPersonalService;
        private readonly IWritePersonalService _writePersonalService;

        public PersonalController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService)
        {
            _readPersonalService = readPersonalService;
            _writePersonalService = writePersonalService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var personals = await _readPersonalService.GetAllWithBranchAndPositionAsync();
            return View(personals);
        }
        [HttpPost]
        public async Task<IActionResult> AddPersonal(AddPersonalDto dto , int pageNumber)
        {
            var result = await _writePersonalService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                NotFound();
            }
            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ArchivePersonal(int id,int pageNumber = 1)
        {
            var result = await _writePersonalService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        
    }
}