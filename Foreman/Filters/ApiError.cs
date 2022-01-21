using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Filters
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object[] StackTrace { get; set; }
        public ApiError InnerError { get; set; }
        

        private static object[] formatStackTrace(Exception e)
        {
            var trace = new System.Diagnostics.StackTrace(e);
            return trace.GetFrames().Select(frame => {
                return new
                {
                    File = frame.GetFileName(),
                    Class = frame.GetType().FullName,
                    Method = frame.GetMethod().ToString(),
                    Line = frame.GetFileLineNumber(),
                    Column = frame.GetFileColumnNumber()
                };
            }).ToArray();
        }

        public static ApiError FromException(Exception e)
        {
            if (e == null)
            {
                return null;
            }

            return new ApiError
            {
                Code = e.HResult,
                Message = e.Message,
                StackTrace = ApiError.formatStackTrace(e),
                InnerError = ApiError.FromException(e.InnerException),
            };
        }
    }
}
