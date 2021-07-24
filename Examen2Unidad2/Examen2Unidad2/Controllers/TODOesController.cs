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
    public class TODOesController : Controller
    {
        private readonly ApplicationDbContext _context;

        //paginacion
        private readonly int RecordsPerPage = 10;
        private Pagination<TODO> PaginationTODO;
        //fin de paginacion
        public TODOesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("/TODO")]
        [Route("/TODO/{search}")]
        [Route("/TODO/{search}/{page}")]

        // GET: TODOes
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }
            //obtener registros totales
            totalRecords = await _context.tODOs.CountAsync(
                s => s.TODOName.Contains(search));
            //obtener datos
            var NAME = await _context.tODOs
                .Where(s => s.TODOName.Contains(search))
                .Include(p => p.project)
                .ToListAsync();
            //paginar
            var NAMEResult = NAME.OrderBy(o => o.TODOName)
                  .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            PaginationTODO = new Pagination<TODO>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = NAMEResult
            };

            return View(PaginationTODO);
        }
        // GET: TODOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tODO = await _context.tODOs.Include(p => p.project)
                .FirstOrDefaultAsync(m => m.TODOid == id);
            if (tODO == null)
            {
                return NotFound();
            }

            return View(tODO);
        }

        // GET: TODOes/Create
        public IActionResult Create()
        {
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME");
            return View();
        }

        // POST: TODOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TODOid,TODOName,TODOstart,TODOfinish,projectID")] TODO tODO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tODO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", tODO.projectID);
            return View(tODO);
        }

        // GET: TODOes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tODO = await _context.tODOs.FindAsync(id);
            if (tODO == null)
            {
                return NotFound();
            }
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", tODO.projectID);
            return View(tODO);
        }

        // POST: TODOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TODOid,TODOName,TODOstart,TODOfinish,projectID")] TODO tODO)
        {
            if (id != tODO.TODOid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tODO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TODOExists(tODO.TODOid))
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
            ViewData["projectID"] = new SelectList(_context.Project, "projectID", "projectNAME", tODO.projectID);
            return View(tODO);
        }

        // GET: TODOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tODO = await _context.tODOs
                .Include(p => p.project)
                .FirstOrDefaultAsync(m => m.TODOid == id);
            if (tODO == null)
            {
                return NotFound();
            }

            return View(tODO);
        }

        // POST: TODOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tODO = await _context.tODOs.FindAsync(id);
            _context.tODOs.Remove(tODO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TODOExists(int id)
        {
            return _context.tODOs.Any(e => e.TODOid == id);
        }
    }
}
