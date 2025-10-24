using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? WorkSchedule { get; set; }

    public virtual ICollection<GroupClass> GroupClasses { get; set; } = new List<GroupClass>();
}
