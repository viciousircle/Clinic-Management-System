using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OOP_Final_Project.Pages.Inventory
{
    public class InventoryManagementModel : PageModel
    {
        public List<Medicine> Medicines { get; set; }

        public void OnGet()
        {
            // Sample data
            Medicines = new List<Medicine>
            {
                new Medicine { Id = 1, Name = "Paracetamol", Type = "Tablet", Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6) },
                new Medicine { Id = 2, Name = "Cough Syrup", Type = "Syrup", Quantity = 50, ExpiryDate = DateTime.Now.AddMonths(12) },
                new Medicine { Id = 3, Name = "Insulin", Type = "Injection", Quantity = 20, ExpiryDate = DateTime.Now.AddMonths(3) }
            };
        }

        public class Medicine
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public int Quantity { get; set; }
            public DateTime ExpiryDate { get; set; }
        }
    }
}
