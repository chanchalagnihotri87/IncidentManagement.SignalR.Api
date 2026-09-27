using IncidentManagement.Domain.Common;
using IncidentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentManagement.Domain.Entities
{
    public class TeamMember: BaseEntity
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }

        public TeamMemberRole Role { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Team Team { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
