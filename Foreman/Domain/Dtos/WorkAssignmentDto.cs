using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foreman.Domain.Models;

namespace Foreman.Domain.Dtos
{
    public class WorkAssignmentDto
    {
        public Worker Worker { get; set; }
        public WorkItemResultDto CurrentWorkItem { get; set; }
    }
}
