using Electronic_Bank.Data;
using Electronic_Bank.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Electronic_Bank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ClientCount"] = _context.Client.Count();
            ViewData["WalletCount"] = _context.Wallet.Count();
            ViewData["TransactionCount"] = _context.Transaction.Count();
            ViewData["CurrencyCount"] = _context.Currency.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}