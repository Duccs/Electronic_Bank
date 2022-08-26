using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Electronic_Bank.Data;
using Electronic_Bank.Models;
using Microsoft.AspNetCore.Authorization;

namespace Electronic_Bank.Controllers
{
    [Authorize]
    public class WalletsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WalletsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "client_desc" : "";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.CurrencySortParm = sortOrder == "Currency" ? "currency_desc" : "Currency";
            var applicationDbContext = from s in _context.Wallet.Include(w => w.Client).Include(w => w.Currency)
                                       select s;

            switch (sortOrder)
            {
                case "client_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.Client.Name);
                    break;
                case "Amount":
                    applicationDbContext = applicationDbContext.OrderBy(s => s.Amount);
                    break;
                case "amount_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.Amount);
                    break;
                case "Currency":
                    applicationDbContext = applicationDbContext.OrderBy(s => s.Currency.Code);
                    break;
                case "currency_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.Currency.Code);
                    break;
                default:
                    applicationDbContext = applicationDbContext.OrderBy(s => s.Client.Name);
                    break;
            }

            var wallets = applicationDbContext.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                //movies = movies.Where(s => s.Title!.Contains(searchString));
                wallets = wallets.FindAll(m => m.Client.Name!.Contains(searchString));

            }
            return View(wallets);
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .Include(w => w.Client)
                .Include(w => w.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: Wallets/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name");
            ViewData["CurrencyId"] = new SelectList(_context.Currency, "Id", "Name");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Amount,CurrencyId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", wallet.ClientId);
            ViewData["CurrencyId"] = new SelectList(_context.Currency, "Id", "Name", wallet.CurrencyId);
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", wallet.ClientId);
            ViewData["CurrencyId"] = new SelectList(_context.Currency, "Id", "Name", wallet.CurrencyId);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Amount,CurrencyId")] Wallet wallet)
        {
            if (id != wallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", wallet.ClientId);
            ViewData["CurrencyId"] = new SelectList(_context.Currency, "Id", "Name", wallet.CurrencyId);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .Include(w => w.Client)
                .Include(w => w.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wallet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wallet'  is null.");
            }
            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallet.Remove(wallet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Deposit(int id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return View(wallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            if (_context.Wallet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wallet'  is null.");
            }
            var wallet = await _context.Wallet.FindAsync(id);
            if (amount != null)
            {
                try
                {
                    Transaction transaction = new Transaction()
                    {
                        Date = DateTime.Now,
                        WalletId = wallet.Id,
                        type = TypeEnum.Deposit,
                        Amount = amount
                    };
                    _context.Transaction.Add(transaction);
                    wallet.Amount += amount;
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
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
            
            return View(wallet);
        }

        public async Task<IActionResult> Withdraw(int id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return View(wallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            if (_context.Wallet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wallet'  is null.");
            }
            var wallet = await _context.Wallet.FindAsync(id);
            if (amount != null)
            {
                try
                {
                    Transaction transaction = new Transaction()
                    {
                        Date = DateTime.Now,
                        WalletId = wallet.Id,
                        type = TypeEnum.Withdraw,
                        Amount = amount
                    };
                    _context.Transaction.Add(transaction);
                    wallet.Amount -= amount;
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
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

            return View(wallet);
        }

        public async Task<IActionResult> Transfer(int id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewBag.TargetWallet = new SelectList(_context.Wallet, "Id", "Id", wallet.Id);
            TransferViewModel transfer = new TransferViewModel()
            {
                Id = wallet.Id
            };
            return View(transfer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(int id, [Bind("Id,target,amount")] TransferViewModel transfer)
        {
            if (_context.Wallet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wallet'  is null.");
            }
            var wallet = await _context.Wallet.FindAsync(id);
            var targetwallet = await _context.Wallet.FindAsync(transfer.target);
            if (transfer.amount != null)
            {
                try
                {
                    Transaction transaction = new Transaction()
                    {
                        Date = DateTime.Now,
                        WalletId = wallet.Id,
                        type = TypeEnum.Transfer,
                        Amount = transfer.amount,
                        Description = "Amount sent to wallet with Id "+targetwallet.Id
                    };
                    _context.Transaction.Add(transaction);
                    targetwallet.Amount += transfer.amount;
                    wallet.Amount -= transfer.amount;
                    _context.Update(wallet);
                    _context.Update(targetwallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id) || !WalletExists(targetwallet.Id))
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

            return View(wallet);
        }

        private bool WalletExists(int id)
        {
          return (_context.Wallet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
