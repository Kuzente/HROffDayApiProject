using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PositionServices;

namespace UI.Components
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
            return await Task.Run(() => View(dto));
        }
    }
}
