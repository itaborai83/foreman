using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Models
{
    public class WorkItem
    {
        [Key]
        [Required]
        public int Id { get; set; }        
        [Required]
        public string Description { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public WorkStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
