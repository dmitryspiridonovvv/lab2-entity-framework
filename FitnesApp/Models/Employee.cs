using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public virtual ICollection<ClientMembership> ClientMemberships { get; set; } = new List<ClientMembership>();
}
