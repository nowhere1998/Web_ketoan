using System;
using System.Collections.Generic;

namespace MyShop.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Company { get; set; }

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public string? Mail { get; set; }

    public string? Detail { get; set; }

    public DateTime? Date { get; set; }

    public int? Active { get; set; }

    public string? Lang { get; set; }

    public string Code { get; set; } = null!;
}
