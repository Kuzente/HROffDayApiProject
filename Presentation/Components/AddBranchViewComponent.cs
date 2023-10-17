using Core.DTOs.BranchDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;

namespace Presentation.Components;

public class AddBranchViewComponent : ViewComponent
{
    

    public IViewComponentResult Invoke(WriteBranchDto dto)
    {
        return View(dto);
    }
    
}