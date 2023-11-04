using Core.DTOs.BranchDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;

namespace Presentation.Components;

public class AddBranchViewComponent : ViewComponent
{
    

    public async Task<IViewComponentResult> InvokeAsync(BranchDto dto)
    {
        return await Task.Run(()=> View(dto));
    }
    
}