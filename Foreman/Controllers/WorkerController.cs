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
using System.Threading.Tasks;

namespace Foreman.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly IWorkService _workService;
        private readonly IMapper _mapper;

        public WorkerController(ILogger<WorkerController> logger, IWorkService workService, IMapper mapper)
        {
            _logger = logger;
            _workService = workService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetWorkers()
        {
            var workers = _workService.GetWorkers();
            return Ok(workers);
        }

        [HttpGet]
        [Route("{workerId}")]
        public IActionResult GetWorker(int workerId)
        {
            if (!ModelState.IsValid)
                return this.BadRequest();

            try
            {
                var worker = _workService.GetWorker(workerId);
                return Ok(worker);
            }
            catch (WorkerNotFound)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateWorker([FromBody]WorkerDto workerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var worker = _workService.CreateWorker(workerDto);
            return this.CreatedAtAction(nameof(GetWorker), new { workerId = worker.Id }, worker);
        }

        [HttpPut]
        [Route("{workerId}")]
        public IActionResult UpdateWorker(int workerId, [FromBody] WorkerDto workerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
        
            try
            {
                var _ = _workService.UpdateWorker(workerId, workerDto);
                return NoContent();
            }
            catch (WorkerNotFound)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{workerId}")]
        public IActionResult DeleteWorker(int workerId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            try
            {
                var _ = _workService.DeleteWorker(workerId);
                return NoContent();
            }
            catch (WorkerNotFound)
            {
                return NotFound();
            }
        }
    }
}