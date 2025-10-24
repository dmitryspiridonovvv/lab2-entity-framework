using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class MembershipType
{
    public int MembershipTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? DurationDays { get; set; }

    public int? VisitCount { get; set; }

    public decimal Cost { get; set; }

    public string AvailableZones { get; set; } = null!;

    public virtual ICollection<ClientMembership> ClientMemberships { get; set; } = new List<ClientMembership>();
}
