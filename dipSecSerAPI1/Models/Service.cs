using System;
using System.Collections.Generic;

namespace dipSecSerAPI1.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? GalleryId { get; set; }

    public virtual Gallery? Gallery { get; set; }
}
