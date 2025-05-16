using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private ICatalogContext _context { get; }

        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        }
        async Task<Product> IProductRepository.GetProduct(string id)
        {
            return await _context
              .Products
              .Find(p => p.Id == id)
              .FirstOrDefaultAsync();
        }

        async Task<Pagination<Product>> IProductRepository.GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter = filter & builder.Eq(p => p.Name.ToLower(), catalogSpecParams.Search);
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
                filter &= brandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
                filter &= typeFilter;
            }
            var totalItems = await _context
                .Products
                .CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpecParams, filter);

            return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, (int)totalItems, data);
        }



        async Task<IEnumerable<Product>> IProductRepository.GetProductsByBrand(string brandName)
        {
            return await _context
                         .Products
                         .Find(p => p.Brand.Name.ToLower() == brandName.ToLower())
                         .ToListAsync();
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProductsByName(string name)
        {
            return await _context
                           .Products
                           .Find(p => p.Name.ToLower() == name.ToLower())
                           .ToListAsync();
        }

        async Task<bool> IProductRepository.UpdateProduct(Product product)
        {
            var updatedProduct = await _context
                                        .Products
                                        .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }
        async Task<Product> IProductRepository.CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        async Task<bool> IProductRepository.DeleteProduct(string id)
        {
            var deleteProduct = await _context
                                     .Products
                                     .DeleteOneAsync(p => p.Id == id);
            return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > 0;
        }

        async Task<IEnumerable<ProductBrand>> IBrandRepository.GetAllBrands()
        {
            return await _context
                         .Brands
                         .Find(b => true)
                         .ToListAsync();
        }

        async Task<IEnumerable<ProductType>> ITypesRepository.GetAllTypes()
        {
            return await _context
                            .Types
                            .Find(t => true)
                            .ToListAsync();
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefn = Builders<Product>.Sort.Ascending("Name");//default
            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefn = Builders<Product>.Sort.Ascending("Name");
                        break;
                }
            }
            return await _context
                .Products
                .Find(filter)
                .Sort(sortDefn)
                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }

    }
}
