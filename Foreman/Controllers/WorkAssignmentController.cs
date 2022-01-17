using AutoMapper;
using Foreman.Domain.Dtos;
using Foreman.Domain.Errors;
using Foreman.Domain.Models;
using Foreman.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Foreman.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkAssignmentController : ControllerBase
    {
        private static ReaderWriterLock foremanLock = new ReaderWriterLock();
        private const int LOCK_READ_TIMEOUT_MS  = 10000;
        private const int LOCK_WRITE_TIMEOUT_MS = 60000;

        private readonly ILogger<WorkAssignmentController> _logger;
        private readonly IWorkService _workService;
        private readonly IMapper _mapper;

        public WorkAssignmentController(ILogger<WorkAssignmentController> logger, IWorkService workService, IMapper mapper)
        {
            _logger = logger;
            _workService = workService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetWorkAssignments()
        {
            try
            {
                foremanLock.AcquireReaderLock(LOCK_READ_TIMEOUT_MS);
                var workAssignments = _workService.GetWorkAssignments();
                return Ok(workAssignments);
            }
            finally
            {
                if (foremanLock.IsReaderLockHeld)
                    foremanLock.ReleaseReaderLock();
            }
        }

        [HttpPost]
        [Route("{workerId}")]
        public IActionResult AssignWork(int workerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                foremanLock.AcquireWriterLock(LOCK_WRITE_TIMEOUT_MS);
                var workAssignment = _workService.AssignWork(workerId);
                return Ok(workAssignment);
            }
            catch(WorkerNotFound)
            {
                return NotFound();
            }
            catch(WorkerAlreadyAssigned waa)
            {
                return BadRequest(waa.Message);
            }
            finally
            {
                if (foremanLock.IsWriterLockHeld)
                    foremanLock.ReleaseWriterLock();
            }
        }

        [HttpPost]
        [Route("{workerId}/notifyDone")]
        public IActionResult NotifyDone(int workerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                foremanLock.AcquireWriterLock(LOCK_WRITE_TIMEOUT_MS);
                var assignment = _workService.NotifyDone(workerId);
                return Ok(assignment);
            }
            catch (WorkerNotFound)
            {
                return NotFound();
            }
            catch (WorkerNotAssigned e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidWorkItemStatus e)
            {
                return BadRequest(e.Message);
            }
            finally
            {
                if (foremanLock.IsWriterLockHeld)
                    foremanLock.ReleaseWriterLock();
            }
        }

        [HttpPost]
        [Route("{workerId}/notifyError")]
        public IActionResult NotifyError(int workerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                foremanLock.AcquireWriterLock(LOCK_WRITE_TIMEOUT_MS);
                var assignment = _workService.NotifyError(workerId);
                return Ok(assignment);
            }
            catch (WorkerNotFound)
            {
                return NotFound();
            }
            catch (WorkerNotAssigned e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidWorkItemStatus e)
            {
                return BadRequest(e.Message);
            }
            finally
            {
                if (foremanLock.IsWriterLockHeld)
                    foremanLock.ReleaseWriterLock();
            }
        }
    }
}