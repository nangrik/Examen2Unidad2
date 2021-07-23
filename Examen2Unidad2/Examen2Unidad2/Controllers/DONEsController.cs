using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen2Unidad2.Data;
using Examen2Unidad2.Models;
using Examen2Unidad2.Common;

namespace Examen2Unidad2.Controllers
{
    public class DONEsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //paginacion
        private readonly int RecordsPerPage = 10;
        private Pagination<DONE> PaginationDONE;
        //fin

        public DONEsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/DONE")]
        [Route("/DONE/{search}")]
        [Route("/DONE/{search}/{page}")]

        // GET: DONEs
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }
            //obtener registros totales
            totalRecords = await _context.dONEs.CountAsync(
                s => s.DONEName.Contains(search));
            //obtener datos
            var NAMES = await _context.dONEs
                .Where(s => s.DONEName.Contains(search)).ToListAsync();
            //paginar
            var DONEResult = NAMES.OrderBy(o => o.DONEName)
                  .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            PaginationDONE = new Pagination<DONE>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = DONEResult
            };

            return View(PaginationDONE);
        }

        // GET: DONEs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dONE = await _context.dONEs
                .FirstOrDefaultAsync(m => m.doneID == id);
            if (dONE == null)
            {
                return NotFound();
            }

            return View(dONE);
        }

        // GET: DONEs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DONEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("doneID,DONEName,doneSTATUS,doneDATE")] DONE dONE)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dONE);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dONE);
        }

        // GET: DONEs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dONE = await _context.dONEs.FindAsync(id);
            if (dONE == null)
            {
                return NotFound();
            }
            return View(dONE);
        }

        // POST: DONEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("doneID,DONEName,doneSTATUS,doneDATE")] DONE dONE)
        {
            if (id != dONE.doneID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dONE);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DONEExists(dONE.doneID))
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
            return View(dONE);
        }

        // GET: DONEs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dONE = await _context.dONEs
                .FirstOrDefaultAsync(m => m.doneID == id);
            if (dONE == null)
            {
                return NotFound();
            }

            return View(dONE);
        }

        // POST: DONEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dONE = await _context.dONEs.FindAsync(id);
            _context.dONEs.Remove(dONE);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DONEExists(int id)
        {
            return _context.dONEs.Any(e => e.doneID == id);
        }
    }
}
