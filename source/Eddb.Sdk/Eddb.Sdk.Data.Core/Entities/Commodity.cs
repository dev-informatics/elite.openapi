namespace Eddb.Sdk.Entities
{
    public class Commodity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public int AveragePrice { get; set; }
        public Category Category { get; set; }
    }
}
