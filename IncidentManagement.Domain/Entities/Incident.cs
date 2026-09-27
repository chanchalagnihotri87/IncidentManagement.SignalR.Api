using IncidentManagement.Domain.Common;
using IncidentManagement.Domain.Enums;
using System;

namespace IncidentManagement.Domain.Entities
{
    public class Incident : BaseEntity
    {
        public Guid PublicId { get; set; }

        public string IncidentNumber { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public IncidentSeverity Severity { get; set; }

        public IncidentPriority Priority { get; set; }

        public IncidentStatus Status { get; set; }

        public IncidentCategory Category { get; set; }

        public IncidentEnvironment Environment { get; set; }

        public Guid ServiceId { get; set; }

        public Guid ReporterId { get; set; }

        public Guid? AssigneeId { get; set; }

        public Guid TeamId { get; set; }

        public DateTime? ResolvedDate { get; set; }


        // Navigation Properties

        public Service Service { get; set; } = null!;

        public User Reporter { get; set; } = null!;

        public User? Assignee { get; set; }

        public Team Team { get; set; } = null!;
    }
}
