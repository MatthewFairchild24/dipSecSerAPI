using System;
using System.Collections.Generic;

namespace dipSecSerAPI1.Models;

public partial class Gallery
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
