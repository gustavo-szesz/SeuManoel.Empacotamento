namespace br.seumanoel.empacotamento.api.Models
{
    public class PackedProduct
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int PackedBoxId { get; set; }
        public PackedBox PackedBox { get; set; }
    }
}