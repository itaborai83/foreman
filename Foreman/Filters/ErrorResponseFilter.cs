using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var apiError = ApiError.FromException(context.Exception);
            var objectResult = new ObjectResult(apiError);
            objectResult.StatusCode = 500;
            context.Result = objectResult;
        }
    }
}
