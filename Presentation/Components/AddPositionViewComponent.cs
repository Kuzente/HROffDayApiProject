using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Components
{
    public class AddPositionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PositionDto dto)
        {
            return await Task.Run(() => View(dto));
        }
    }
}
