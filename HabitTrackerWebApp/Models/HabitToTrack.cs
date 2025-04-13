using System;
using System.Security.Cryptography.X509Certificates;

namespace HabitTrackerWebApp.Models;

public class HabitToTrack
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
