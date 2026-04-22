using System.Net.Http.Json;
using MGM.GoodHamburger.App.Models;

namespace MGM.GoodHamburger.App.Services;

public class MenuService
{
    private readonly HttpClient _httpClient;

    public MenuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MenuItemDto>> GetAllMenuItemsAsync()
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<MenuItemDto>>("api/menu");
            return items ?? new List<MenuItemDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar itens do menu: {ex.Message}");
            return new List<MenuItemDto>();
        }
    }
}
