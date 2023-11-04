using Microsoft.AspNetCore.Mvc;

namespace UI.Components;

public class DeletedPositionPaginationViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        
        return  await Task.Run(()=> View());
    }
}