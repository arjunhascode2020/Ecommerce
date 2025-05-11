using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private   ICatalogContext _context { get; }

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

        async Task<IEnumerable<Product>> IProductRepository.GetProducts()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
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

      
    }
}
