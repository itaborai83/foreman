using Foreman.Domain.Dtos;
using Foreman.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Services
{
    public interface IWorkService
    {
        public IEnumerable<Worker> GetWorkers();
        public Worker GetWorker(int workerId);
        public Worker CreateWorker(WorkerDto worker);
        public Worker UpdateWorker(int workerId, WorkerDto worker);
        public Worker DeleteWorker(int workerId);
        
        public IEnumerable<WorkItemResultDto> GetWorkItems();
        public IEnumerable<WorkItemResultDto> GetWorkItems(WorkStatus status);
        public WorkItemResultDto GetWorkItem(int workItemId);
        public WorkItemResultDto CreateWorkItem(WorkItemDto workItemDto);
        public WorkItemResultDto UpdateWorkItem(int workItemId, WorkItemDto workItemDto);
        public WorkItemResultDto DeleteWorkItem(int workItemId);

        public IEnumerable<WorkAssignmentDto> GetWorkAssignments();
        public WorkAssignmentDto AssignWork(int workerId);
        public WorkAssignmentDto NotifyDone(int workerId);
        public WorkAssignmentDto NotifyError(int workerId);
    }
}
