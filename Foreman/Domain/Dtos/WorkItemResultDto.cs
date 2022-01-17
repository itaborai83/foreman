using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Dtos
{
    public class WorkItemResultDto
    {
        private DateTime? _queriedAt;

        public int Id { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime QueriedAt 
        {
            get 
            { 
                // race condition
                if (!_queriedAt.HasValue)
                {
                    _queriedAt = DateTime.Now;
                }
                return _queriedAt.Value;
            }
        }
        public int? Duration
        {
            get
            {
                if (StartedAt.HasValue && EndedAt.HasValue)
                {
                    return (int)EndedAt.Value.Subtract(StartedAt.Value).TotalSeconds;
                }
                else if (StartedAt.HasValue)
                {
                    return (int)QueriedAt.Subtract(StartedAt.Value).TotalSeconds;
                }
                else
                { 
                    return null;
                }
            }
        }
    }
}
