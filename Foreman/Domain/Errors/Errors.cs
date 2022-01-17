using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Errors
{
    public class ApiError : Exception
    {
        public ApiError() { }
        public ApiError(string message) : base(message) { }
        public ApiError(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkerError : ApiError
    {
        public WorkerError() {}
        public WorkerError(string message) : base(message) {}
        public WorkerError(string message, Exception inner) : base(message, inner) {}
    }

    public class WorkerNotFound : WorkerError
    {
        public WorkerNotFound() { }
        public WorkerNotFound(string message) : base(message) { }
        public WorkerNotFound(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkItemError : ApiError
    {
        public WorkItemError() { }
        public WorkItemError(string message) : base(message) { }
        public WorkItemError(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkItemNotFound : WorkItemError
    {
        public WorkItemNotFound() { }
        public WorkItemNotFound(string message) : base(message) { }
        public WorkItemNotFound(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkAssignmentError : ApiError
    {
        public WorkAssignmentError() { }
        public WorkAssignmentError(string message) : base(message) { }
        public WorkAssignmentError(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkerAlreadyAssigned : WorkAssignmentError
    {
        public WorkerAlreadyAssigned() { }
        public WorkerAlreadyAssigned(string message) : base(message) { }
        public WorkerAlreadyAssigned(string message, Exception inner) : base(message, inner) { }
    }

    public class WorkerNotAssigned : WorkAssignmentError
    {
        public WorkerNotAssigned() { }
        public WorkerNotAssigned(string message) : base(message) { }
        public WorkerNotAssigned(string message, Exception inner) : base(message, inner) { }
    }

    public class NoWorkItemsRemaining : WorkAssignmentError
    {
        public NoWorkItemsRemaining() { }
        public NoWorkItemsRemaining(string message) : base(message) { }
        public NoWorkItemsRemaining(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidWorkItemStatus : WorkAssignmentError
    {
        public InvalidWorkItemStatus() { }
        public InvalidWorkItemStatus(string message) : base(message) { }
        public InvalidWorkItemStatus(string message, Exception inner) : base(message, inner) { }
    }


}
