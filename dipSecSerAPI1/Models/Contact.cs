using System;
using System.Collections.Generic;

namespace dipSecSerAPI1.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string WorkTime { get; set; } = null!;
}
