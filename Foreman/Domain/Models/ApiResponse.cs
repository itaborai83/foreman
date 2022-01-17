using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Models
{
    public enum StatusCode
    {
        OK                      = 1 << 0,
        CREATED                 = 1 << 1,
        UPDATED                 = 1 << 2,
        DELETED                 = 1 << 3,
        ERROR                   = 1 << 4,
        NOT_FOUND               = 1 << 5,
        ALREADY_ASSIGNED        = 1 << 6,
        NOT_ASSIGNED            = 1 << 7,
        NO_WORK_ITEMS_LEFT      = 1 << 8,
        INVALID_WORKER_STATE    = 1 << 9
    }

    public class ApiResponse<T>
    {
        public string __Type { get { return nameof(T);  } }
        
        public StatusCode Status { get; set; }
        
        public string Message { get; set; }
        
        public string Location { get; set; }
        public T Payload { get; set; }

        public bool Error { 
            get { 
                switch(Status)
                {
                    case StatusCode.OK: 
                    case StatusCode.CREATED: 
                    case StatusCode.UPDATED:
                    case StatusCode.DELETED:
                        return false;
                    default:
                        return true;
                }
            }
        }

        public IActionResult AsControllerResponse()
        {
            switch(Status)
            {
                case StatusCode.OK: 
                case StatusCode.DELETED:
                case StatusCode.UPDATED:
                    return new OkObjectResult(this);
                
                case StatusCode.CREATED:
                    if (String.IsNullOrEmpty(this.Location))
                    {
                        throw new ArgumentException("missing location");
                    }
                    return new CreatedResult(this.Location, this);

                case StatusCode.NOT_FOUND:
                    return new NotFoundObjectResult(this);

                case StatusCode.ERROR:
                case StatusCode.ALREADY_ASSIGNED:
                case StatusCode.NOT_ASSIGNED:
                case StatusCode.NO_WORK_ITEMS_LEFT:
                case StatusCode.INVALID_WORKER_STATE:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
