using Microsoft.AspNetCore.Mvc;

namespace br.seumanoel.empacotamento.api.Models
{
    public class PackingResult
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public List<PackedBox> Boxes { get; set; }
    }
}
