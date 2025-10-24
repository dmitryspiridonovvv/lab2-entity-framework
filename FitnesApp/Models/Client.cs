using System;
using System.Collections.Generic;

namespace FitnesApp.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public byte[]? Photo { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<ClassSignup> ClassSignups { get; set; } = new List<ClassSignup>();

    public virtual ICollection<ClientMembership> ClientMemberships { get; set; } = new List<ClientMembership>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
