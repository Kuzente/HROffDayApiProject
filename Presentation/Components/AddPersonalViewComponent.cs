using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PositionServices;

namespace Presentation.Components
{
    public class AddPersonalViewComponent : ViewComponent
    {
        private readonly IReadBranchService _readBranchService;
        private readonly IReadPositionService _readPositionService;

        public AddPersonalViewComponent(IReadBranchService readBranchService, IReadPositionService readPositionService)
        {
            _readBranchService = readBranchService;
            _readPositionService = readPositionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(AddPersonalDto dto)
        {
            ViewBag.Positions = await _readPositionService.GetAllJustNames();
            ViewBag.Branches = await _readBranchService.GetAllJustNames();
                  
            return await Task.Run(() => View(dto));
        }
    }
}
