using MGM.GoodHamburger.App.Models;
using System.Net.Http.Json;

namespace MGM.GoodHamburger.App.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderResponse?> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/order", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderResponse>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar pedido: {response.StatusCode} - {errorContent}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao conectar com a API: {ex.Message}", ex);
        }
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/order");

            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<OrderResponse>>();
                return orders ?? new List<OrderResponse>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar pedidos: {response.StatusCode} - {errorContent}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao conectar com a API: {ex.Message}", ex);
        }
    }
}

public class CreateOrderRequest
{
    public int? SandwichId { get; set; }
    public int? SideDishId { get; set; }
    public int? DrinkId { get; set; }
}

public class OrderResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public MenuItemDto? Sandwich { get; set; }
    public MenuItemDto? SideDish { get; set; }
    public MenuItemDto? Drink { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
}
