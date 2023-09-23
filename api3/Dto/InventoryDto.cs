﻿using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace api3.Dto;

public partial class InventoryDto
{
    public int IdInventory { get; set; }

    public int IdStore { get; set; }

    public int IdEmployee { get; set; }

    public DateTime Date { get; set; }

    public string? Flavor { get; set; }

    public string? IsSeasonFlavor { get; set; }

    public int? Quantity { get; set; }

    //public virtual EmployeeDto? IdEmployeeNavigation { get; set; }

    //public virtual StoreDto? IdStoreNavigation { get; set; }
}

public partial class InventoryUpdateDto
{
    [Name("Listed By")]
    public string store { get; set; }

   // public int IdEmployee { get; set; }

    //public DateTime? Date { get; set; } 

    //public string? Flavor { get; set; }

    //public string? IsSeasonFlavor { get; set; }

    public int Quantity { get; set; }

}

