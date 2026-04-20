namespace MGM.GoodHamburger.Domain.Services;

public static class DiscountCalculator
{
    public static decimal CalculateDiscount(bool hasSandwich, bool hasSideDish, bool hasDrink)
    {
        if (hasSandwich && hasSideDish && hasDrink)
            return 0.20m; // 20%
        
        if (hasSandwich && hasDrink)
            return 0.15m; // 15%
        
        if (hasSandwich && hasSideDish)
            return 0.10m; // 10%
        
        return 0m;
    }
}