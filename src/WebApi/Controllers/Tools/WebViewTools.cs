namespace IAVH.BioTablero.CM.WebApi.Controllers.Tools;

using System;
using System.IO;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

/// <summary>
/// Web View Tools.
/// </summary>
/// <param name="logger">System logger.</param>
/// <param name="serviceProvider">Service provider.</param>
public class WebViewTools(
    ILogger logger,
    IServiceProvider serviceProvider) : IWebViewTools
{
    private readonly ILogger logger = logger;
    private readonly IServiceProvider serviceProvider = serviceProvider;

    /// <summary>
    /// Cast Web View to string.
    /// </summary>
    /// <param name="viewName">View name.</param>
    /// <param name="model">View model data.</param>
    /// <param name="isPartial">Partial view flag.</param>
    /// <returns>Web view as HTML string.</returns>
    public async Task<string> RenderViewToString(
        string viewName,
        object model,
        bool isPartial = false)
    {
        // Code based in: https://weblog.west-wind.com/posts/2022/Jun/21/Back-to-Basics-Rendering-Razor-Views-to-String-in-ASPNET-Core
        var routeData = new RouteData();
        routeData.Values.Add("controller", "EmailTemplates");

        using var requestServices = serviceProvider.CreateScope();
        var httpContext = new DefaultHttpContext { RequestServices = requestServices.ServiceProvider };
        var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

        var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
        var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

        using var stringWriter = new StringWriter();
        var viewResult = razorViewEngine.FindView(actionContext, viewName, !isPartial);

        if (viewResult?.View == null)
        {
            logger.Error("Razor view render error: {@viewName} view not found", viewName);
            throw new ArgumentException($"{viewName} does not match any available view");
        }

        var viewDictionary =
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            { Model = model };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
            stringWriter,
            new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);
        return stringWriter.ToString();
    }
}
