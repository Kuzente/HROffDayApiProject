using Microsoft.AspNetCore.Mvc;

namespace Presentation.Components;

public class PaginationViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        
         return  await Task.Run(()=> View());
    }
}