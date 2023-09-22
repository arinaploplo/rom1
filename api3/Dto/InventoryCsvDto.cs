namespace api3.Dto
{
    public class InventoryCsvDto
    {
        public string Store { get; set; }
        public string Date { get; set; }
        public string Flavor { get; set; }
        public string IsSeasonFlavor { get; set; }
        public int Quantity { get; set; }
        public string ListedBy { get; set; }
    }
}
