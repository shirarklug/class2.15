using HW2._15.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HW2._15.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            var vm = new ToDoItemsViewModel();
            vm.ToDoItems = mgr.GetToDoItems();
            return View(vm);
        }

        public IActionResult Completed()
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            var vm = new ToDoItemsViewModel();
            vm.ToDoItems = mgr.GetCompletedToDoItems();
            return View(vm);
        }

        public IActionResult Categories()
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            var vm = new CategoriesViewModel();
            vm.Categories = mgr.GetCategories();
            return View(vm);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public IActionResult AddedCategory(string categoryName)
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            mgr.AddCategory(categoryName);
            return Redirect("/Home/Categories");
        }

        public IActionResult MarkAsCompleted(int ItemId)
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            mgr.MarkAsCompleted(ItemId);
            return Redirect("/Home/Completed");
        }

        public IActionResult AddedItem(string title, DateTime dueDate, int categoryId)
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            mgr.AddItem(title, dueDate, categoryId);
            return Redirect("/Home/Index");
        }

        public IActionResult AddItem()
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDo; Integrated Security=true;TrustServerCertificate=True";
            var mgr = new ToDoManager(connectionString);
            var vm = new CategoriesViewModel();
            vm.Categories = mgr.GetCategories();
            return View(vm);
        }
    }
}
