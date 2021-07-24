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
    public class DOINGsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //paginacion
        private readonly int RecordsPerPage = 10;
        private Pagination<DOING> PaginationDOING;
        //fin

        public DOINGsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //busqueda
        [Route("/DOING")]
        [Route("/DOING/{search}")]
        [Route("/DOING/{search}/{page}")]
        //fin del cosdigo

        // GET: DOINGs
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }
            //obtener registros totales
            totalRecords = await _context.dOINGs.CountAsync(
                s => s.doingName.Contains(search));
            //obtener datos
            var NAME = await _context.dOINGs
                .Where(s => s.doingName.Contains(search))
                .Include(p => p.project)
                .ToListAsync();
            //paginar
            var NAMEResult = NAME.OrderBy(o => o.doingName)
                  .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            PaginationDOING = new Pagination<DOING>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = NAMEResult
            };

            return View(PaginationDOING);
        }


        // GET: DOINGs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dOING = await _context.dOINGs.Include(p => p.project)
                .FirstOrDefaultAsync(m => m.doingID == id);
            if (dOING == null)
            {
                return NotFound();
            }

            return View(dOING);
        }

        // GET: DOINGs/Create
        public IActionResult Create()
        {
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME");
            return View();
        }

        // POST: DOINGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("doingID,doingName,doingSTATUS,projectID")] DOING dOING)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dOING);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", dOING.projectID);
            return View(dOING);
        }

        // GET: DOINGs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dOING = await _context.dOINGs.FindAsync(id);
            if (dOING == null)
            {
                return NotFound();
            }
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", dOING.projectID);
            return View(dOING);
        }

        // POST: DOINGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("doingID,doingName,doingSTATUS,projectID")] DOING dOING)
        {
            if (id != dOING.doingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dOING);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DOINGExists(dOING.doingID))
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
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", dOING.projectID);
            return View(dOING);
        }

        // GET: DOINGs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dOING = await _context.dOINGs
                .Include(p => p.project)
                .FirstOrDefaultAsync(m => m.doingID == id);
            if (dOING == null)
            {
                return NotFound();
            }

            return View(dOING);
        }

        // POST: DOINGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dOING = await _context.dOINGs.FindAsync(id);
            _context.dOINGs.Remove(dOING);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DOINGExists(int id)
        {
            return _context.dOINGs.Any(e => e.doingID == id);
        }
    }
}
