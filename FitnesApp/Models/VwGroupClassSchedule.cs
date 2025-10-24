using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class VwGroupClassSchedule
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string Schedule { get; set; } = null!;

    public string TrainerName { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public int MaxParticipants { get; set; }
}
