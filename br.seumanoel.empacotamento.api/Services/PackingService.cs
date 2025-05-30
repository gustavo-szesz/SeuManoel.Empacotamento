using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;

namespace br.seumanoel.empacotamento.api.Services
{
    public class PackingService
    {
        private readonly IBoxFactory _boxFactory;
        public PackingService(IBoxFactory boxFactory)
        {
            _boxFactory = boxFactory;
        }


        public List<OrderPackedDto> PackOrders(List<OrderDto> orders)
        {
            var result = new List<OrderPackedDto>();

            foreach (var order in orders)
            {
                var availableBoxes = _boxFactory.GetAllBoxes();
                var packedBoxes = new List<PackedBoxDto>();
                // Ordena produtos do maior para o menor
                var productsToPack = order.Products
                    .OrderByDescending(p => p.Dimensions.Height * p.Dimensions.Width * p.Dimensions.Length)
                    .ToList();

                foreach (var box in availableBoxes.OrderBy(b => b.Volume))
                {
                    // Enquanto houver produtos que caibam nesta caixa, continue abrindo novas caixas desse tipo
                    while (productsToPack.Any(p =>
                        new Product(p.ProductId, p.Dimensions.Height, p.Dimensions.Width, p.Dimensions.Length).Height <= box.Height &&
                        new Product(p.ProductId, p.Dimensions.Height, p.Dimensions.Width, p.Dimensions.Length).Width <= box.Width &&
                        new Product(p.ProductId, p.Dimensions.Height, p.Dimensions.Width, p.Dimensions.Length).Length <= box.Length))
                    {
                        var selectedBox = new Box(box.Height, box.Width, box.Length, box.Name);
                        var packedBox = new PackedBoxDto
                        {
                            BoxName = PackedBoxDto.CastNameBox(box.Name),
                            PackedProductIds = new List<string>(),
                            Observation = null
                        };

                        bool addedAny = false;

                        // Tente adicionar produtos um a um até não caber mais
                        for (int i = 0; i < productsToPack.Count;)
                        {
                            var p = productsToPack[i];
                            var productEntity = new Product(
                                p.ProductId,
                                p.Dimensions.Height,
                                p.Dimensions.Width,
                                p.Dimensions.Length
                            );

                            if (selectedBox.CanHave(productEntity))
                            {
                                packedBox.PackedProductIds.Add(p.ProductId);
                                selectedBox.AddProduct(productEntity);
                                productsToPack.RemoveAt(i);
                                addedAny = true;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        if (packedBox.PackedProductIds.Count > 0)
                        {
                            packedBoxes.Add(packedBox);
                        }

                        // Se não adicionou nenhum produto, pare de abrir caixas deste tipo
                        if (!addedAny)
                            break;
                    }
                }


                // Se ainda restaram produtos, eles não cabem em nenhuma caixa
                while (productsToPack.Count > 0)
                {
                    var notPacked = productsToPack.First();
                    packedBoxes.Add(new PackedBoxDto
                    {
                        BoxName = null,
                        PackedProductIds = new List<string> { notPacked.ProductId },
                        Observation = "Produto não cabe em nenhuma caixa disponível."
                    });
                    productsToPack.RemoveAt(0);
                }

                result.Add(new OrderPackedDto
                {
                    OrderId = order.OrderId,
                    Boxes = packedBoxes
                });
            }

            return result;
        }
    }
}
