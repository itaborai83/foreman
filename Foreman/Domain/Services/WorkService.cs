using AutoMapper;
using Foreman.Domain.Dtos;
using Foreman.Domain.Errors;
using Foreman.Domain.Models;
using Foreman.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Services
{
    public class WorkService : IWorkService
    {

        private readonly ILogger<IWorkService> _logger;
        private readonly IWorkRepository _workRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkService(
            ILogger<IWorkService> logger, 
            IWorkRepository workRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _workRepository = workRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private Worker EnsureWorkerExists(int workerId)
        {
            var worker = _workRepository.GetWorker(workerId);
            if (worker == null)
            {
                throw new WorkerNotFound($"could not find worker #{workerId}");
            }
            return worker;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return _workRepository.GetWorkers();
        }

        public Worker GetWorker(int workerId)
        {
            return EnsureWorkerExists(workerId);
        }

        public Worker CreateWorker(WorkerDto workerDto)
        {
            var worker = _mapper.Map<Worker>(workerDto);
            _workRepository.CreateWorker(worker);
            _unitOfWork.Complete();
            return worker;
        }
        
        public Worker UpdateWorker(int workerId, WorkerDto workerDto)
        {
            var worker = EnsureWorkerExists(workerId);
            _mapper.Map(workerDto, worker);
            _workRepository.UpdateWorker(worker);
            _unitOfWork.Complete();
            return worker;
        }

        public Worker DeleteWorker(int workerId)
        {
            var worker = EnsureWorkerExists(workerId);
            _workRepository.DeleteWorker(worker);
            _unitOfWork.Complete();
            return worker;
        }

        public IEnumerable<WorkItemResultDto> GetWorkItems()
        {
            return _workRepository.GetWorkItems().Select(wi => _mapper.Map<WorkItemResultDto>(wi));
        }

        public IEnumerable<WorkItemResultDto> GetWorkItems(WorkStatus status)
        {
            return _workRepository.GetWorkItems(status).Select(wi => _mapper.Map<WorkItemResultDto>(wi));
        }

        public WorkItemResultDto GetWorkItem(int workItemId)
        {
            var workItem = _workRepository.GetWorkItem(workItemId);
            if (workItem == null)
            {
                throw new WorkItemError($"could not find work item #{workItemId}");
            }
            return _mapper.Map<WorkItemResultDto>(workItem);
        }

        public WorkItemResultDto CreateWorkItem(WorkItemDto workItemDto)
        {
            var workItem = _mapper.Map<WorkItem>(workItemDto);
            workItem.CreatedAt = DateTime.Now;
            workItem.Status = WorkStatus.CREATED;
            _workRepository.CreateWorkItem(workItem);
            _unitOfWork.Complete();
            return _mapper.Map<WorkItemResultDto>(workItem);
        }

        private WorkItem EnsureWorkItemExists(int workItemId)
        {
            var workItem = _workRepository.GetWorkItem(workItemId);
            if (workItem == null)
            {
                throw new WorkItemNotFound($"could not find worker #{workItemId}");
            }
            return workItem;
        }

        public WorkItemResultDto UpdateWorkItem(int workItemId, WorkItemDto workItemDto)
        {
            var workItem = EnsureWorkItemExists(workItemId);
            _mapper.Map(workItemDto, workItem);
            _workRepository.UpdateWorkItem(workItem);
            _unitOfWork.Complete();
            return _mapper.Map<WorkItemResultDto>(workItem);
        }

        public WorkItemResultDto DeleteWorkItem(int workItemId)
        {
            var workItem = EnsureWorkItemExists(workItemId);
            _workRepository.DeleteWorkItem(workItem);
            _unitOfWork.Complete();
            return _mapper.Map<WorkItemResultDto>(workItem);
        }

        public IEnumerable<WorkAssignmentDto> GetWorkAssignments()
        {
            var assignments = new List<WorkAssignmentDto>();
            var workers = _workRepository.GetWorkers().ToList();
            // N+1 anti pattern... using it only because there will never be a huge number of workers
            foreach(Worker worker in workers)
            {
                if (!worker.CurrentWorkId.HasValue)
                {
                    continue;
                }
                var workItem = _workRepository.GetWorkItem(worker.CurrentWorkId.Value);
                var workAssignment = new WorkAssignmentDto
                {
                    Worker = worker,
                    CurrentWorkItem = _mapper.Map<WorkItemResultDto>(workItem)
                };
                assignments.Add(workAssignment);
            }
            return assignments;
        }

        public WorkAssignmentDto AssignWork(int workerId)
        {
            var worker = EnsureWorkerExists(workerId);
            if (worker.CurrentWorkId.HasValue)
            {
                var errorMsg = $"worker is already assigned to work item # {worker.CurrentWorkId}";
                throw new WorkerAlreadyAssigned(errorMsg);
            }

            // Race condition
            var workItem = _workRepository.FirstUnassignedWorkItem();
            if (workItem == null)
            {
                return new WorkAssignmentDto
                {
                    Worker = worker,
                    CurrentWorkItem = null
                };
            }

            worker.CurrentWorkId = workItem.Id;
            workItem.Status = WorkStatus.RUNNING;
            workItem.StartedAt = DateTime.Now;
            _workRepository.UpdateWorker(worker);
            _workRepository.UpdateWorkItem(workItem);
            _unitOfWork.Complete();

            return new WorkAssignmentDto
            {
                Worker = worker,
                CurrentWorkItem = _mapper.Map<WorkItemResultDto>(workItem)
            };            
        }

        private WorkAssignmentDto FinishWorkItem(int workerId, WorkStatus status)
        {
            if (status != WorkStatus.FINISHED && status != WorkStatus.CANCELLED)
            {
                throw new ArgumentException($"invalid status received: {status.ToString()}");
            }

            var worker = EnsureWorkerExists(workerId);

            if (!worker.CurrentWorkId.HasValue)
            {
                var errorMsg = $"worker does not have a work item assigned to it";
                throw new WorkerNotAssigned(errorMsg);
            }

            var workItem = _workRepository.GetWorkItem(worker.CurrentWorkId.Value);
            if (workItem == null)
            {
                // TODO: Add FK
                worker.CurrentWorkId = null;
                _workRepository.UpdateWorker(worker);
                _unitOfWork.Complete();
                var errorMsg = $"worker does not have a valid work item assigned to it";
                throw new WorkerNotAssigned(errorMsg);
            }

            if (workItem.Status != WorkStatus.RUNNING)
            {
                var workStatus = workItem.Status.ToString();
                var errorMsg = $"worker assigned to work item #{workItem.Id} with an invalid status: '{workStatus}'";
                throw new InvalidWorkItemStatus(errorMsg);
            }

            worker.CurrentWorkId = null;
            workItem.Status = status;
            workItem.EndedAt = DateTime.Now;
            _workRepository.UpdateWorker(worker);
            _workRepository.UpdateWorkItem(workItem);
            _unitOfWork.Complete();

            return new WorkAssignmentDto
            {
                Worker = worker, 
                CurrentWorkItem = null
            };
        }

        public WorkAssignmentDto NotifyDone(int workerId)
        {
            return FinishWorkItem(workerId, WorkStatus.FINISHED);
        }

        public WorkAssignmentDto NotifyError(int workerId)
        {
            return FinishWorkItem(workerId, WorkStatus.CANCELLED);
        }
    }
}
