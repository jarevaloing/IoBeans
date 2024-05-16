using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoBeans.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using IoBeans.Recursos;
using Microsoft.AspNetCore.Authentication;

namespace IoBeans.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IoBeansContext _context;

        public LoginsController(IoBeansContext context)
        {
            _context = context;
        }

        // GET: Logins
        public async Task<IActionResult> Index(int? pageNumber, int pageSize = 10)
        {
            var items = _context.Logins.AsNoTracking();
            int totalCount = await items.CountAsync();
            int currentPage = pageNumber ?? 1;
            var pagedItems = await items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new PaginatedList<Login>(pagedItems, totalCount, currentPage, pageSize);
            return View(model);
        }

        // GET: Logins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .Include(l => l.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        // GET: Logins/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,RoleId")] Login login)
        {
            if (ModelState.IsValid)
            {
                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", login.RoleId);
            return View(login);
        }

        // GET: Logins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }
            var roles = _context.Roles.ToListAsync().Result;
            ViewBag.roles = roles.Select(p => new SelectListItem() { Value = p.RoleId.ToString(), Text = p.RoleName }).ToList<SelectListItem>();

            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Login login)
        {
            if (id != login.UserId)
            {
                return NotFound();
            }

            var Usuario = _context.Logins.FindAsync(id).Result;

            Usuario.RoleId = login.RoleId;
            Usuario.Password = Utilidades.EncriptarClave(login.Password);



            if (Usuario.UserId == id)
            {
                try
                {
                    _context.Update(Usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginExists(Usuario.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", login.RoleId);
            return View(login);
        }

        // GET: Logins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .Include(l => l.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Logins == null)
            {
                return Problem("Entity set 'IoBeansContext.Logins' is null.");
            }
            var login = await _context.Logins.FindAsync(id);
            if (login != null)
            {
                _context.Logins.Remove(login);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginExists(int id)
        {
            return (_context.Logins?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("IniciarSesion", "Inicio");
        }

    }
}
