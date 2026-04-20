using MGM.GoodHamburger.Domain.Entities;

namespace MGM.GoodHamburger.Domain.Repositories;

public interface IMenuItemRepository
{
    Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken);
    Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
}