using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    class GetProductByBrandQuery:IRequest<IList<ProductResponse>>
    {
        public string BrandName { get; set; }
        public GetProductByBrandQuery(string brandName)
        {
            BrandName = brandName;
        }
    }
}
