using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var personals = await _readPersonalService.GetAllWithBranchAndPositionAsync();
            return View(personals);
        }
    }
}