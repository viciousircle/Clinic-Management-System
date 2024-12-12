using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OOP_Final_Project.Models;

namespace OOP_Final_Project.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:5298");
            var response = await client.GetAsync("/api/employees");
            if (response.IsSuccessStatusCode)
            {
                var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
            }
        }
    }

}
