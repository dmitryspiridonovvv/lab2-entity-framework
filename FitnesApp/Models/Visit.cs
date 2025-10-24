using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int ClientId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public virtual Client Client { get; set; } = null!;
}
