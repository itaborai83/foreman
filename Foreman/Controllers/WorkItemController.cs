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
    public class WorkItemController : ControllerBase
    {
        private readonly ILogger<WorkItemController> _logger;
        private readonly IWorkService _workService;
        private readonly IMapper _mapper;

        public WorkItemController(ILogger<WorkItemController> logger, IWorkService workService, IMapper mapper)
        {
            _logger = logger;
            _workService = workService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetWorkItems()
        {
            var workItems = _workService.GetWorkItems();
            return Ok(workItems);
        }

        
        [HttpGet]
        [Route("{workItemId}")]
        public IActionResult GetWorkItem(int workItemId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var workItem = _workService.GetWorkItem(workItemId);
                return Ok(workItem);
            }
            catch (WorkItemNotFound)
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        public IActionResult CreateWorkItem([FromBody]WorkItemDto workItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var workItemResultDto = _workService.CreateWorkItem(workItemDto);
            return this.CreatedAtAction(nameof(GetWorkItem), new { workItemId = workItemResultDto.Id }, workItemResultDto);
        }

        [HttpDelete]
        [Route("{workItemId}")]
        public IActionResult DeleteWorker(int workItemId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var _ = _workService.DeleteWorkItem(workItemId);
                return NoContent();
            }
            catch (WorkItemNotFound)
            {
                return NotFound();
            }
        }

        
        [HttpPut]
        [Route("{workItemId}")]
        public IActionResult UpdateWorker(int workItemId, [FromBody] WorkItemDto workItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var _ = _workService.UpdateWorkItem(workItemId, workItemDto);
                return NoContent();
            }
            catch (WorkItemNotFound)
            {
                return NotFound();
            }
        }
    }
}