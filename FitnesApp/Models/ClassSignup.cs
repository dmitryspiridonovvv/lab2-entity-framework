using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class ClassSignup
{
    public int SignupId { get; set; }

    public int ClientId { get; set; }

    public int ClassId { get; set; }

    public DateTime SignupDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual GroupClass Class { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
