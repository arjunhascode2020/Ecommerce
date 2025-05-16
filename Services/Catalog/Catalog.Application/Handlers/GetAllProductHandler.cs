using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        async Task<Pagination<ProductResponse>> IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>.Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecParams);
            var productResponseList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
            return productResponseList;
        }
    }
}
