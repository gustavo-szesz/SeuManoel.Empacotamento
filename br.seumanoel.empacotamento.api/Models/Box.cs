namespace br.seumanoel.empacotamento.api.Models
{
    public class Box
    {
        // The proprieties of Box class (Height, Width, Length, Name)
        // are going to be parsed to *altura*, *largura* and *comprimento*
        // in the DTO's, to match the JSON output structure in the example (saida.json)
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; } = new();

        public int Volume => Height * Width * Length;
        public int RemainingVolume => 
            Volume - Products.Sum(p => p.Height * p.Width * p.Length);

        public Box(int height, int width, int length, string name)
        {
            Height = height;
            Width = width;
            Length = length;
            Name = name;
        }

        
        public bool CanHave(Product p)
        {
            return p.Height <= Height &&
                   p.Width <= Width &&
                   p.Length <= Length &&
                   p.CalculateVolume() <= RemainingVolume;
        }

        public void AddProduct(Product p) => Products.Add(p);
    }
}
