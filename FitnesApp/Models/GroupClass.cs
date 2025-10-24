using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class GroupClass
{
    public int ClassId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TrainerId { get; set; }

    public string Schedule { get; set; } = null!;

    public int MaxParticipants { get; set; }

    public virtual ICollection<ClassSignup> ClassSignups { get; set; } = new List<ClassSignup>();

    public virtual Trainer Trainer { get; set; } = null!;
}
