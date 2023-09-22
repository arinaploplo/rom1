using System;
using System.Collections.Generic;

namespace api3.Models;

public partial class Inventory
{
    public int IdInventory { get; set; }

    public int? IdStore { get; set; }

    public int? IdEmployee { get; set; }

    public DateTime? Date { get; set; }

    public string? Flavor { get; set; }

    public string? IsSeasonFlavor { get; set; }

    public int? Quantity { get; set; }

    public virtual Employee? IdEmployeeNavigation { get; set; }

    public virtual Store? IdStoreNavigation { get; set; }
}
