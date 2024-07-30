using InMemoryCaching.Application.Dtos;
using InMemoryCaching.Domain.Entities;
using InMemoryCaching.Domain.Repositories;
using InMemoryCaching.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(AddProductDto product)
        {
            if(product != null)
            {
                var prod = new Product(product.Name!, product.Description!, product.Price);
                _context.Products.Add(prod);

                _context.SaveChanges();
            }
        }

        public async Task<List<Product>?> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);

            return products;
        }


        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var products = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

            return products;
        }

    }
}
