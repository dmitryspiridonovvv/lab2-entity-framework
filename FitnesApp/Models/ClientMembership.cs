using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class ClientMembership
{
    public int ClientMembershipId { get; set; }

    public int ClientId { get; set; }

    public int MembershipTypeId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual MembershipType MembershipType { get; set; } = null!;
}
