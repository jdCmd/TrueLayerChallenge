using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;

namespace TrueLayerChallenge.WebApi.Extensions;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Handler used to compose an error response for global exceptions.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    /// <param name="logger">Logger for logging unhandled exceptions.</param>
    public static void AddExceptionHandling(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    logger.LogError(statusCode, errorFeature.Error, errorFeature.Error.Message);
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync("Internal server error occurred.");
            });
        });
    }
}