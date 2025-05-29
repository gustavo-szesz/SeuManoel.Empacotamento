namespace br.seumanoel.empacotamento.api.Models
{
    public class PackedBox
    {
        public int Id { get; set; }
        public string BoxName { get; set; }
        public string? Observation { get; set; }
        public int PackingResultId { get; set; }
        public PackingResult PackingResult { get; set; }
        public List<PackedProduct> Products { get; set; }
    }
}