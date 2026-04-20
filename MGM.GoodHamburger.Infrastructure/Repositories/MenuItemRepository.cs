using Microsoft.EntityFrameworkCore;
using MGM.GoodHamburger.Domain.Entities;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Infrastructure.Data;

namespace MGM.GoodHamburger.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.MenuItems.ToListAsync(cancellationToken);
    }

    public async Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
    }
}