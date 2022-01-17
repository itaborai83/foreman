using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Dtos
{
    public class WorkItemDto
    {

        [Required]
        public string Description { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
