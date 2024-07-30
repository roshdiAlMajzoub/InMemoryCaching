using InMemoryCaching.Application.Dtos;
using InMemoryCaching.Domain.Entities;
using InMemoryCaching.Domain.Repositories;
using InMemoryCaching.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching.Infrastructure.Repositories
{
    public class CachedProductRepository : IProductRepository
    {
        private readonly ProductRepository _decorated;
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _context;

        public CachedProductRepository(ProductRepository decorated, IMemoryCache memoryCache, ApplicationDbContext context)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
            _context = context;
        }

        void IProductRepository.Add(AddProductDto product)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>?> GetAllAsync(CancellationToken cancellationToken)
        {
            string key = "products";

            if(!_memoryCache.TryGetValue(key, out List<Product> cachedData))
            {
                cachedData = await _decorated.GetAllAsync(cancellationToken);

                _memoryCache.Set(key, cachedData, TimeSpan.FromMinutes(2));
            }


            return cachedData;
        }

        Task<Product?> IProductRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            string key = $"member-{id}";

            return _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetByIdAsync(id, cancellationToken);
                }
                );
        }
    }
}
