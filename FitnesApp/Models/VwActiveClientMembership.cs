using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class VwActiveClientMembership
{
    public int ClientId { get; set; }

    public string ClientFullName { get; set; } = null!;

    public string MembershipName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
