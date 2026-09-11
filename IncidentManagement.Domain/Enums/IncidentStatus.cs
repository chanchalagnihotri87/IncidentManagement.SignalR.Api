using System;
using System.Collections.Generic;
using System.Text;

namespace IncidentManagement.Domain.Enums
{
    public enum IncidentStatus
    {
        New,
        Assigned,
        InProgress,
        OnHold,
        Resolved,
        Closed,
        Cancelled
    }
}
