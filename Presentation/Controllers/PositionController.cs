using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PositionServices;

namespace Presentation.Controllers
{
    public class PositionController : Controller
    {
        private readonly IReadPositionService _readPositionService;
        private readonly IWritePositionService _writePositionService;

        public PositionController(IReadPositionService readPositionService, IWritePositionService writePositionService)
        {
            _readPositionService = readPositionService;
            _writePositionService = writePositionService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            var resultSearch = await _readPositionService.GetAllPagingOrderByAsync(pageNumber, search);
            return View(resultSearch);
        }
        [HttpPost]
        public async Task<IActionResult> AddPosition(PositionDto dto, int pageNumber)
        {
            var result = await _writePositionService.AddAsync(dto);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePosition(int id, int pageNumber)
        {
            var result = await _readPositionService.GetByIdUpdate(id);

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePosition(ResultWithDataDto<PositionDto> dto, int pageNumber = 1)
        {
            var result = await _writePositionService.UpdateAsync(dto.Data);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ArchivePosition(int id, int pageNumber = 1)
        {
            var result = await _writePositionService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Index", new { pageNumber = pageNumber });
            }

            return BadRequest("Index");
        }
    }
}
