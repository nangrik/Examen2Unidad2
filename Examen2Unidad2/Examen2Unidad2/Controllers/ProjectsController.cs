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
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        //paginacion
        private readonly int RecordsPerPage = 10;
        private Pagination<Project> Paginationproject;
        //fin

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //busqueda
        [Route("/project")]
        [Route("/project/{search}")]
        [Route("/project/{search}/{page}")]
        //fin del cosdigo

        // GET: Projects
        public async Task<IActionResult> Index(string search, int page=1)
        {
            int totalRecords = 0;
            if (search == null)
            {
                search = "";
            }
            //obtener registros totales
            totalRecords = await _context.Project.CountAsync(
                s => s.projectNAME.Contains(search));
            //obtener datos
            var NAME = await _context.Project
                .Where(s => s.projectNAME.Contains(search)).ToListAsync();
            //paginar
            var NAMEResult = NAME.OrderBy(o => o.projectNAME)
                  .Skip((page - 1) * RecordsPerPage)
                .Take(RecordsPerPage);
            //calculo de paginas
            var totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
            //instanciar paginacion
            Paginationproject = new Pagination<Project>()
            {
                RecordPerPage = this.RecordsPerPage,
                TotalRecords = totalRecords,
                TotalPage = totalPages,
                CurrentPage = page,
                Search = search,
                Result = NAMEResult
            };

            return View(Paginationproject);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.projectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("projectID,projectNAME")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("projectID,projectNAME")] Project project)
        {
            if (id != project.projectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.projectID))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.projectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.projectID == id);
        }
    }
}
