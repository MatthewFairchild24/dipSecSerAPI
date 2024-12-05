using System;
using System.Collections.Generic;

namespace dipSecSerAPI1.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly Data { get; set; }

    public int Rating { get; set; }
}
