using System;
using System.Collections.Generic;

namespace dipSecSerAPI1.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ComplitionDate { get; set; }

    public int StatusId { get; set; }

    public int GalleryId { get; set; }

    public virtual Gallery Gallery { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
