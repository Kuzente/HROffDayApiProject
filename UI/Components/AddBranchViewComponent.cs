using Core.DTOs.BranchDTOs;
using Microsoft.AspNetCore.Mvc;


namespace UI.Components;

public class AddBranchViewComponent : ViewComponent
{
    

    public async Task<IViewComponentResult> InvokeAsync(BranchDto dto)
    {
        return await Task.Run(()=> View(dto));
    }
    
}