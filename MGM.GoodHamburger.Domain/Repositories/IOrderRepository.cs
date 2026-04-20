using MGM.GoodHamburger.Domain.Entities;

namespace MGM.GoodHamburger.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}