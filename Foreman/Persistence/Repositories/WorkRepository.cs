using Foreman.Domain.Models;
using Foreman.Domain.Repositories;
using Foreman.Persistence.Contexts.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Persistence.Repositories
{
    public class WorkRepository : IWorkRepository
    {
        private readonly AppDbContext _context;
        
        public WorkRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return _context.Workers.ToList<Worker>();
        }

        public Worker GetWorker(int workerId)
        {
            return _context.Workers.Find(workerId);
        }

        public void CreateWorker(Worker worker)
        {
            _context.Workers.Add(worker);
        }

        public void UpdateWorker(Worker worker)
        {
            _context.Workers.Update(worker);
        }

        public void DeleteWorker(Worker worker)
        {
            _context.Workers.Remove(worker);
        }

        public IEnumerable<WorkItem> GetWorkItems()
        {
            return _context.WorkItems.ToList<WorkItem>();
        }
        
        public IEnumerable<WorkItem> GetWorkItems(WorkStatus status)
        {
            return _context.WorkItems.Where(w => w.Status == status).ToList<WorkItem>();
        }

        public WorkItem GetWorkItem(int workItemId)
        {
            return _context.WorkItems.Find(workItemId);
        }

        public void CreateWorkItem(WorkItem workItem)
        {
            _context.WorkItems.Add(workItem);
        }

        public void UpdateWorkItem(WorkItem workItem)
        {
            _context.WorkItems.Update(workItem);
        }

        public void DeleteWorkItem(WorkItem workItem)
        {
            _context.WorkItems.Remove(workItem);
        }

        public WorkItem FirstUnassignedWorkItem()
        {
            return _context.WorkItems.Where(wi => wi.Status == WorkStatus.CREATED).FirstOrDefault();
        }
    }
}
