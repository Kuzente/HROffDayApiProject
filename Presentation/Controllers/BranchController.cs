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
        public async Task<IActionResult> Index(string search,int pageNumber = 1)
        {
                var resultSearch =  await _readBranchService.GetAllPagingOrderByAsync(pageNumber , search);
                return View(resultSearch);
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(BranchDto dto, int pageNumber)
        {
           var result = await _writeBranchService.AddAsync(dto);

            return RedirectToAction("Index",new { pageNumber = pageNumber });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBranch(int id, int pageNumber)
        {
            var result = await _readBranchService.GetByIdUpdate(id);
           
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(ResultWithDataDto<BranchDto> dto, int pageNumber = 1)
        {
            var result = await _writeBranchService.UpdateAsync(dto.Data);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(int id,int pageNumber = 1)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Index", new { pageNumber = pageNumber });
            }

            return BadRequest("Index");
        }
    }
}