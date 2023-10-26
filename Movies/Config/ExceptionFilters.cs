using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.Exceptions;

namespace Movies.Config;

public class NotFound404ExceptionFilters: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not NotFound404Exception) return;
        context.Result = new NotFoundObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }
}

public class Duplicate409ConflictException : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is not Duplicate409Exception) return;
        context.Result = new ConflictObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }
}

public class BadRequest400BadException : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is not BadRequest400Exception) return;
        context.Result = new BadRequestObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }
}