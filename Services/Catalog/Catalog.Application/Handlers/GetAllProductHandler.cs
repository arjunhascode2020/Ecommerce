using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        async Task<IList<ProductResponse>> IRequestHandler<GetAllProductQuery, IList<ProductResponse>>.Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts();
            var productResponseList = ProductMapper.Mapper.Map<IList<ProductResponse>>(productList);
            return productResponseList;
        }
    }
}
