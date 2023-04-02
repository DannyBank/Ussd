using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Models;

public class UssdContent : IActionResult
{
    public string? Content { get; set; }
    public bool AllowInput { get; set; }
    public string MenuHeader { get; set; } = string.Empty;

    public UssdContent(string? content, bool allowInput = false, string menu = "")
    {
        Content = content;
        AllowInput = allowInput;
        MenuHeader = menu;
        ContentType = "text/plain; charset=utf-8";
    }

    public UssdContent()
    {
        ContentType = "text/plain; charset=utf-8";
    }

    private string ContentType { get; set; }

    #region Implementation of IActionResult

    /// <inheritdoc />
    public virtual async  Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        if (Content == null)
            Content = string.Empty;
        Content = $"{(AllowInput ? StringConstants.APLI_FC : StringConstants.APLI_FB)},{Content}";
        context.HttpContext.Response.Headers.Add("Menu", MenuHeader);
        var executor = context.HttpContext.RequestServices.GetRequiredService <IHttpResponseStreamWriterFactory >();
        await using var textWriter = executor.CreateWriter(response.Body, Encoding.UTF8);
        await textWriter.WriteAsync(Content?.Trim());

        // Flushing the HttpResponseStreamWriter does not flush the underlying stream. This just flushes
        // the buffered text in the writer.
        // We do this rather than letting dispose handle it because dispose would call Write and we want
        // to call WriteAsync.
        await textWriter.FlushAsync();
    }

    #endregion
}