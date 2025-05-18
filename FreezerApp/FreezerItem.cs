namespace FreezerApp.Models
{
    public class FreezerItem
    {
        public Guid Id { get; set; }
        public int? BoxId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Location { get; set; }
        public DateTime StoreDate { get; set; }
    }
}
