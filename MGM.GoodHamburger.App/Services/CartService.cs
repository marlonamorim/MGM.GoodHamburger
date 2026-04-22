using MGM.GoodHamburger.App.Models;

namespace MGM.GoodHamburger.App.Services;

public class CartService
{
    private readonly List<CartItem> _items = new();

    public event Action? OnChange;

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public int ItemCount => _items.Sum(i => i.Quantity);

    public decimal Subtotal => _items.Sum(i => i.MenuItem.Price * i.Quantity);

    public decimal DiscountPercentage => CalculateDiscountPercentage();

    public decimal DiscountAmount => Subtotal * (DiscountPercentage / 100);

    public decimal Total => Subtotal - DiscountAmount;

    private decimal CalculateDiscountPercentage()
    {
        var hasSandwich = _items.Any(i => i.MenuItem.Type.Equals("Sandwich", StringComparison.OrdinalIgnoreCase));
        var hasSideDish = _items.Any(i => i.MenuItem.Type.Equals("SideDish", StringComparison.OrdinalIgnoreCase));
        var hasDrink = _items.Any(i => i.MenuItem.Type.Equals("Drink", StringComparison.OrdinalIgnoreCase));

        // 20% - Combo Completo (Hambúrguer + Acompanhamento + Bebida)
        if (hasSandwich && hasSideDish && hasDrink)
            return 20;

        // 15% - Hambúrguer + Bebida
        if (hasSandwich && hasDrink)
            return 15;

        // 10% - Hambúrguer + Acompanhamento
        if (hasSandwich && hasSideDish)
            return 10;

        return 0;
    }

    public void AddItem(MenuItemDto menuItem)
    {
        var existingItem = _items.FirstOrDefault(i => i.MenuItem.Id == menuItem.Id);
        
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            _items.Add(new CartItem
            {
                MenuItem = menuItem,
                Quantity = 1
            });
        }

        NotifyStateChanged();
    }

    public void RemoveItem(int menuItemId)
    {
        var item = _items.FirstOrDefault(i => i.MenuItem.Id == menuItemId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void UpdateQuantity(int menuItemId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.MenuItem.Id == menuItemId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                RemoveItem(menuItemId);
            }
            else
            {
                item.Quantity = quantity;
                NotifyStateChanged();
            }
        }
    }

    public void Clear()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    public (int? SandwichId, int? SideDishId, int? DrinkId) GetItemIdsForOrder()
    {
        var sandwichId = _items
            .FirstOrDefault(i => i.MenuItem.Type.Equals("Sandwich", StringComparison.OrdinalIgnoreCase))
            ?.MenuItem.Id;

        var sideDishId = _items
            .FirstOrDefault(i => i.MenuItem.Type.Equals("SideDish", StringComparison.OrdinalIgnoreCase))
            ?.MenuItem.Id;

        var drinkId = _items
            .FirstOrDefault(i => i.MenuItem.Type.Equals("Drink", StringComparison.OrdinalIgnoreCase))
            ?.MenuItem.Id;

        return (sandwichId, sideDishId, drinkId);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

public class CartItem
{
    public MenuItemDto MenuItem { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Subtotal => MenuItem.Price * Quantity;
}
