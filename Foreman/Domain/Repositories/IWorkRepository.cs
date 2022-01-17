using Foreman.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Repositories
{
    public interface IWorkRepository
    {
        public IEnumerable<Worker> GetWorkers();
        public Worker GetWorker(int workerId);
        public void CreateWorker(Worker worker);
        public void UpdateWorker(Worker worker);
        public void DeleteWorker(Worker worker);
        public IEnumerable<WorkItem> GetWorkItems();
        public IEnumerable<WorkItem> GetWorkItems(WorkStatus status);
        public WorkItem GetWorkItem(int workItemId);
        public void CreateWorkItem(WorkItem workItem);
        public void UpdateWorkItem(WorkItem workItem);
        public void DeleteWorkItem(WorkItem workItem);
        public WorkItem FirstUnassignedWorkItem();
    }
}
