using Microsoft.AspNetCore.Mvc;
using TrueLayerChallenge.WebApi.Resources;

namespace TrueLayerChallenge.WebApi.Controllers;

/// <summary>
/// Base controller class providing common controller functionality.
/// </summary>
/// <typeparam name="TController"> is the type of the inheriting <see cref="ApiController{T}"/>.</typeparam>
[ApiController]
[Route("[controller]")]
public abstract class ApiController<TController> : ControllerBase where TController : ApiController<TController>
{
    private readonly ILogger<TController> _logger;

    /// <summary>
    /// Creates a new <see cref="ApiController{TController}"/>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance for the <see cref="ApiController{TController}"/> instance to perform logging.</param>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> is null.</exception>
    protected ApiController(ILogger<TController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Asynchronously performs the given <paramref name="func"/> whilst providing common logging and exception handling functionality.
    /// </summary>
    /// <typeparam name="T">Return type of <paramref name="func"/>.</typeparam>
    /// <param name="executingControllerActionName">The name of the controller action calling the method. This should be provided using the nameof method.</param>
    /// <param name="func">The main logic of the controller action to perform.</param>
    /// <returns><see cref="ActionResult{T}"/>.</returns>
    protected async Task<ActionResult<T>> PerformFuncAsync<T>(string executingControllerActionName, Func<Task<ActionResult<T>>> func)
    {
        try
        {
            _logger.Log(LogLevel.Information, LogMessages.Api_Called, executingControllerActionName);
            var result = await func();
            _logger.Log(LogLevel.Information, LogMessages.Api_Succeeded, executingControllerActionName);
            return result;
        }
        // todo add further exception handling as required
        catch (ArgumentException e)
        {
            _logger.Log(LogLevel.Error, LogMessages.Api_Failed, executingControllerActionName, e.Message);
            return BadRequest();
        }
        catch (Exception e)
        {
            // Log critical as never expected to occur.
            _logger.Log(LogLevel.Critical, LogMessages.Api_Failed, executingControllerActionName, e.Message);
            // On unhandled exception return InternalServerError.
            return StatusCode(500);
        }
        finally
        {
            _logger.Log(LogLevel.Information, LogMessages.Api_Complete, executingControllerActionName);
        }
    }
}