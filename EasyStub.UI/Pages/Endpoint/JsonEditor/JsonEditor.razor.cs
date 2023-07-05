namespace EasyStub.UI.Pages.Endpoint.JsonEditor;

using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;

public partial class JsonEditor : ComponentBase, IAsyncDisposable
{
    private StandaloneCodeEditor editor = null!;

    public async Task<string> GetValueAsync() => await editor.GetValue();
    public async Task SetValueAsync(string newValue) => await editor.SetValue(newValue);

    private const string DefaultValue = "{}";

    private static StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor _) => new()
    {
        AutomaticLayout = true,
        Language = "json",
        Value = DefaultValue,
        Theme = "vs-dark"
    };

    public async ValueTask DisposeAsync()
    {
        await editor.SetValue(DefaultValue);
        GC.SuppressFinalize(this);
    }
}