using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class VwClientVisitHistory
{
    public string ClientFullName { get; set; } = null!;

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public int? DurationMinutes { get; set; }
}
