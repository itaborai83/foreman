using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Models
{
    public enum WorkStatus
    {
        CREATED = 1,
        RUNNING = 2,
        FINISHED = 3,
        CANCELLED = 4
    }

    public static class WorkStatusMethods
    {
        public static readonly string[] Names = new string[] { "Created", "Running", "Finished", "Cancelled" };

        public static int ToInt(this WorkStatus w)
        {
            switch (w)
            {
                case WorkStatus.CREATED: return 1;
                case WorkStatus.RUNNING: return 2;
                case WorkStatus.FINISHED: return 3;
                case WorkStatus.CANCELLED: return 4;
                default: throw new InvalidOperationException("Should not be reached");
            }
        }

        public static String ToString(this WorkStatus w)
        {
            switch(w)
            {
                case WorkStatus.CREATED: return Names[0];
                case WorkStatus.RUNNING: return Names[1];
                case WorkStatus.FINISHED: return Names[2];
                case WorkStatus.CANCELLED: return Names[3];
                default: throw new InvalidOperationException("Should not be reached");
            }
        }

        public static WorkStatus FromString(string s)
        {
            switch (s)
            {
                case "Created": return WorkStatus.CREATED;
                case "Running": return WorkStatus.RUNNING;
                case "Finished": return WorkStatus.FINISHED;
                case "Cancelled": return WorkStatus.CANCELLED;
                default: throw new ArgumentException(s);
            }
        }
    }
}
