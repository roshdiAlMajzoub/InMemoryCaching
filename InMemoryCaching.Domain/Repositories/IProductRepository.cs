using InMemoryCaching.Application.Dtos;
using InMemoryCaching.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Product>?> GetAllAsync(CancellationToken cancellationToken = default);

        void Add(AddProductDto product);
    }
}
