using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntryExamsApp.Models;
using EntryExamsApp.Models.Data;

namespace EntryExamsApp.Controllers
{
    public class ExamsController : Controller
    {
        private readonly EntryExamsDbContext _context;

        public ExamsController(EntryExamsDbContext context)
        {
            _context = context;
        }

        // загрузка данных в select'ы
        private void LoadSelectLists(int selectedDiscipline, int selectedEnrollee, int selectedExaminer)
        {
            ViewData["Disciplines"] = new SelectList(_context.Disciplines, "Id", "Name", selectedDiscipline);

            ViewData["Enrollees"] = new SelectList(_context.Enrollees
                .Select(e => new { e.Id, FullName = $"{e.Surname} {e.Name} {e.Patronymic}"}),
                "Id", "FullName", selectedEnrollee);

            ViewData["Examiners"] = new SelectList(_context.Examiners
                .Select(e => new { e.Id, FullName = $"{e.Surname} {e.Name} {e.Patronymic}"}), 
                "Id", "FullName", selectedExaminer);
        }

        private void LoadSelectLists()
        {
            ViewData["Disciplines"] = new SelectList(_context.Disciplines, "Id", "Name");

            ViewData["Enrollees"] = new SelectList(_context.Enrollees
                .Select(e => new { e.Id, FullName = $"{e.Surname} {e.Name} {e.Patronymic}" }),
                "Id", "FullName");

            ViewData["Examiners"] = new SelectList(_context.Examiners
                .Select(e => new { e.Id, FullName = $"{e.Surname} {e.Name} {e.Patronymic}" }),
                "Id", "FullName");
        }

        // GET: Exams
        public async Task<IActionResult> Index()
        {
            var entryExamsDbContext = _context.Exams.Include(e => e.Discipline).Include(e => e.Enrollee).Include(e => e.Examiner);
            return View(await entryExamsDbContext.ToListAsync());
        }

        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Discipline)
                .Include(e => e.Enrollee)
                .Include(e => e.Examiner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: Exams/Create
        public IActionResult Create()
        {
            LoadSelectLists();

            return View();
        }

        // POST: Exams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mark,Date,EnrolleeId,ExaminerId,DisciplineId")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadSelectLists();

            return View(exam);
        }

        // GET: Exams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            LoadSelectLists(exam.DisciplineId, exam.EnrolleeId, exam.ExaminerId);

            return View(exam);
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mark,Date,EnrolleeId,ExaminerId,DisciplineId")] Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
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

            LoadSelectLists(exam.DisciplineId, exam.EnrolleeId, exam.ExaminerId);

            return View(exam);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .Include(e => e.Discipline)
                .Include(e => e.Enrollee)
                .Include(e => e.Examiner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
