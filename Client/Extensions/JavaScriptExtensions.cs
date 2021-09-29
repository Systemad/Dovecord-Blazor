using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Dovecord.Client.Extensions
{
    public static class JavaScriptExtensions
    {
        public static async ValueTask ScrollIntoViewAsync(
            this IJSRuntime javaScript) =>
            await javaScript.InvokeVoidAsync("app.scroll");
    }
}