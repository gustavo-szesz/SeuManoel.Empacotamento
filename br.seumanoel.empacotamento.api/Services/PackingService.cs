using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;

namespace br.seumanoel.empacotamento.api.Services
{
    public class PackingService
    {
        #region Constructor
        private readonly IBoxFactory _boxFactory;
        public PackingService(IBoxFactory boxFactory)
        {
            _boxFactory = boxFactory;
        }
        #endregion

        #region Breve explicação (PT-BR)
        // !!!! IMPORTANTE !!!! 
        // Tanto o README.md quanto essa explicação do algoritmo são escritos em português,
        // por fins de clareza na explicação do algoritmo, 
        // o resto do código está em inglês para seguir as boas práticas.

        // Essa implementacao do algoritmo é beaseado no algoritmo First Fit Decreasing - Bin Packing
        // https://en.wikipedia.org/wiki/First-fit_bin_packing 
        // Não está incluso rotacao de produtos, por motivos de complexidade exponencial
        // Para mais informações, veja o vídeo abaixo: Box Packing is Hard - Keegan R - UWCS
        //   - https://www.youtube.com/watch?v=E9WCI_kkO6k&ab_channel=UWCS-UniversityofWarwickComputingSociety
        // **************************************** Explicacao **********************************************
        // Inicializacao:
        //  Recebe e armazena uma lista de tipos de caixas disponíveis pela Factory 
        //  Cria uma lista vazia para ir armazendo as caixas
        //  Ordena os produtos por volume em ordem decrescente de volume (SortProductsByVolume)
        // Seleção de caixas e empacotamento: (PackProductsIntoBoxType)
        //  Em um loop(foerach) aninhado a um loop condicional(while) | (HasProductsThatFitInBox),
        //  tenta adicionar os produtos um a um na caixa que o foerach está iterando.
        //  Se nenhum produto couber, passa para a próxima caixa disponível.
        // Verificação de encaixe: (PackProductsIntoBox)
        //  Dentro do loop, verifica se o produto cabe na caixa atual (CanHave).
        //  Se couber, adiciona o produto à caixa e remove da lista de produtos a serem empacotados.
        //  Se não couber, continua para o próximo produto.
        // Tratamento de produtos não encaixados: (HandleOversizedProducts)
        //  Após tentar encaixar todos os produtos em todas as caixas disponíveis, 
        //  verifica se ainda há produtos que não foram encaixados.
        //  Para cada produto que não couber, 
        //  cria uma caixa com o nome null e adiciona o produto a ela,
        //  Adiciona uma observacão indicando que o produto não coube em nenhuma caixa disponível.
        // Retorno:
        //  As caixas empacotadas são organizadas 
        //  por ID de pedido e retornadas como solução completa


        #endregion

        /// <summary>
        /// Algorithm to pack orders into boxes using First Fit Decreasing approach
        /// </summary>
        /// <param name="orders">List of orders to process</param>
        /// <returns>Optimized packing solution for each order</returns>
        public List<OrderPackedDto> PackOrders(List<OrderDto> orders)
        {
            var result = new List<OrderPackedDto>();

            foreach (var order in orders)
            {
                var packedBoxes = PackSingleOrder(order);

                result.Add(new OrderPackedDto
                {
                    OrderId = order.OrderId,
                    Boxes = packedBoxes
                });
            }

            return result;
        }

        /// <summary>
        /// Pack products from a single order into appropriate boxes
        /// </summary>
        private List<PackedBoxDto> PackSingleOrder(OrderDto order)
        {
            var availableBoxes = _boxFactory.GetAllBoxes();
            var packedBoxes = new List<PackedBoxDto>();
            
            var productsToPack = SortProductsByVolume(order.Products);

            foreach (var boxType in availableBoxes.OrderBy(b => b.Volume))
            {
                PackProductsIntoBoxType(boxType, productsToPack, packedBoxes);
            }

            HandleOversizedProducts(productsToPack, packedBoxes);
            
            return packedBoxes;
        }

        /// <summary>
        /// Sort products in decreasing order of volume
        /// </summary>
        private List<ProductDto> SortProductsByVolume(List<ProductDto> products)
        {
            return products
                .OrderByDescending(p =>
                            p.Dimensions.Height * p.Dimensions.Width * p.Dimensions.Length)
                .ToList();
        }

        /// <summary>
        /// Try to pack products into boxes of a specific type
        /// </summary>
        private void PackProductsIntoBoxType(Box boxType, List<ProductDto> productsToPack, List<PackedBoxDto> packedBoxes)
        {
            while (HasProductsThatFitInBox(productsToPack, boxType))
            {
                var selectedBox = new Box(boxType.Height, boxType.Width, boxType.Length, boxType.Name);
                var packedBox = new PackedBoxDto
                {
                    BoxName = PackedBoxDto.CastNameBox(boxType.Name),
                    PackedProductIds = new List<string>(),
                    Observation = null
                };

                bool addedAny = PackProductsIntoBox(selectedBox, productsToPack, packedBox);

                if (packedBox.PackedProductIds.Count > 0)
                {
                    packedBoxes.Add(packedBox);
                }

                // If couldn't add any products, stop creating boxes of this type
                if (!addedAny)
                    break;
            }
        }

        /// <summary>
        /// Check if any products can fit in the given box
        /// </summary>
        private bool HasProductsThatFitInBox(List<ProductDto> products, Box box)
        {
            return products.Any(p =>
                new Product(p.ProductId, p.Dimensions.Height,
                            p.Dimensions.Width, p.Dimensions.Length).Height <= box.Height &&
                new Product(p.ProductId, p.Dimensions.Height,
                            p.Dimensions.Width, p.Dimensions.Length).Width <= box.Width &&
                new Product(p.ProductId, p.Dimensions.Height,
                            p.Dimensions.Width, p.Dimensions.Length).Length <= box.Length);
        }

        /// <summary>
        /// Try to pack as many products as possible into a single box
        /// </summary>
        /// <returns>True if at least one product was packed</returns>
        private bool PackProductsIntoBox(Box box, List<ProductDto> productsToPack, PackedBoxDto packedBoxDto)
        {
            bool addedAny = false;

            for (int i = 0; i < productsToPack.Count;)
            {
                var p = productsToPack[i];
                var productEntity = CreateProductEntity(p);

                if (box.CanHave(productEntity))
                {
                    packedBoxDto.PackedProductIds.Add(p.ProductId);
                    box.AddProduct(productEntity);
                    productsToPack.RemoveAt(i);
                    addedAny = true;
                }
                else
                {
                    i++;
                }
            }

            return addedAny;
        }

        /// <summary>
        /// Create a product entity from a product DTO
        /// </summary>
        private Product CreateProductEntity(ProductDto productDto)
        {
            return new Product(
                productDto.ProductId,
                productDto.Dimensions.Height,
                productDto.Dimensions.Width,
                productDto.Dimensions.Length
            );
        }

        /// <summary>
        /// Handle products that don't fit in any available box
        /// And generete one Observation in this case
        /// </summary>
        private void HandleOversizedProducts(List<ProductDto> productsToPack, List<PackedBoxDto> packedBoxes)
        {
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
        }
        
    }
}
