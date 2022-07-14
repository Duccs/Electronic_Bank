using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Electronic_Bank.Data;
using Electronic_Bank.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Electronic_Bank.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hosting;

        public ClientsController(ApplicationDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string searchString)
        {
            var clients = _context.Client.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                //movies = movies.Where(s => s.Title!.Contains(searchString));
                clients = clients.FindAll(m => m.Name!.Contains(searchString));

            }
            return _context.Client != null ? 
                          View(clients) :
                          Problem("Entity set 'ApplicationDbContext.Client'  is null.");
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["Wallets"] = _context.Wallet.Include(w => w.Currency).ToList().FindAll(w => w.ClientId == id);

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
                if (client.Image != null)
                {
                    //ImageUploading
                    string ImageFolder = Path.Combine(hosting.WebRootPath, "imgs");
                    string ImagePath = Path.Combine(ImageFolder, client.Image.FileName);
                    client.Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                    client.ImagePath = client.Image.FileName;
                }
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            try
            {
                if (client.Image != null)
                {

                    //ImageUploading
                    string ImageFolder = Path.Combine(hosting.WebRootPath, "imgs");
                    string ImagePath = Path.Combine(ImageFolder, client.Image.FileName);
                    client.Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                    client.ImagePath = client.Image.FileName;

                }
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Client?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
