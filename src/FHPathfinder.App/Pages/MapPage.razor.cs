using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FHPathfinder.App.Pages;

public partial class MapPage
{
    [Inject]
    private IJSRuntime _js { get; set; }

    private ElementReference MapElement { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await _js.InvokeVoidAsync("App.initMapPage", MapElement);
        }
    }
}