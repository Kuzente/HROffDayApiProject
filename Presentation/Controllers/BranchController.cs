using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;

namespace Presentation.Controllers
{
    public class BranchController : Controller
    {
        private readonly IReadBranchService _readBranchService;
        private readonly IWriteBranchService _writeBranchService;

        public BranchController(IReadBranchService readBranchService, IWriteBranchService writeBranchService)
        {
            _readBranchService = readBranchService;
            _writeBranchService = writeBranchService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1)
        {
            var result =  await _readBranchService.GetAllOrderByAsync();
            
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(WriteBranchDto dto)
        {
           var result = await _writeBranchService.AddAsync(dto);

            return RedirectToAction("Index",result);
        }
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(int id)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));    
            }

            return BadRequest();
        }
    }
}