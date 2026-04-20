using Microsoft.EntityFrameworkCore;
using MGM.GoodHamburger.Domain.Entities;
using MGM.GoodHamburger.Domain.Repositories;
using MGM.GoodHamburger.Infrastructure.Data;

namespace MGM.GoodHamburger.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Sandwich)
            .Include(o => o.SideDish)
            .Include(o => o.Drink)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Sandwich)
            .Include(o => o.SideDish)
            .Include(o => o.Drink)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}