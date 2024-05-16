using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoBeans.Models;

namespace IoBeans.Controllers
{
    public class SensorDatumsController : Controller
    {
        private readonly IoBeansContext _context;

        public SensorDatumsController(IoBeansContext context)
        {
            _context = context;
        }

        // GET: SensorDatums
        public async Task<IActionResult> Index(int? pageNumber, int pageSize = 10)
        {
            var items = _context.SensorData.AsNoTracking();
            int totalCount = await items.CountAsync();
            int currentPage = pageNumber ?? 1;
            var pagedItems = await items.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new PaginatedList<SensorDatum>(pagedItems, totalCount, currentPage, pageSize);
            return View(model);
        }

        // GET: SensorDatums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SensorData == null)
            {
                return NotFound();
            }

            var sensorDatum = await _context.SensorData
                .FirstOrDefaultAsync(m => m.ReadingId == id);
            if (sensorDatum == null)
            {
                return NotFound();
            }

            return View(sensorDatum);
        }

        // GET: SensorDatums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SensorDatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SensorId,Temperature,Humidity,SoilMoisture,Timestamp")] SensorDatum sensorDatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sensorDatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sensorDatum);
        }

        // GET: SensorDatums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SensorData == null)
            {
                return NotFound();
            }

            var sensorDatum = await _context.SensorData.FindAsync(id);
            if (sensorDatum == null)
            {
                return NotFound();
            }
            return View(sensorDatum);
        }

        // POST: SensorDatums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReadingId,SensorId,Temperature,Humidity,SoilMoisture,Timestamp")] SensorDatum sensorDatum)
        {
            if (id != sensorDatum.ReadingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sensorDatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorDatumExists(sensorDatum.ReadingId))
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
            return View(sensorDatum);
        }

        // GET: SensorDatums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SensorData == null)
            {
                return NotFound();
            }

            var sensorDatum = await _context.SensorData
                .FirstOrDefaultAsync(m => m.ReadingId == id);
            if (sensorDatum == null)
            {
                return NotFound();
            }

            return View(sensorDatum);
        }

        // POST: SensorDatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SensorData == null)
            {
                return Problem("Entity set 'IoBeansContext.SensorData' is null.");
            }
            var sensorDatum = await _context.SensorData.FindAsync(id);
            if (sensorDatum != null)
            {
                _context.SensorData.Remove(sensorDatum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SensorDatumExists(int id)
        {
            return (_context.SensorData?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
