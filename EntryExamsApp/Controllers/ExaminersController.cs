using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntryExamsApp.Models;
using EntryExamsApp.Models.Data;
using EntryExamsApp.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EntryExamsApp.Controllers
{
    public class ExaminersController : Controller
    {
        private readonly EntryExamsDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExaminersController(EntryExamsDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Examiners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Examiners.ToListAsync());
        }

        // GET: Examiners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examiner == null)
            {
                return NotFound();
            }

            return View(examiner);
        }

        // GET: Examiners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Examiners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamPrice,Id,Surname,Name,Patronymic,Photo")] ExaminerViewModel examinerViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(examinerViewModel);
                Examiner examiner = new Examiner
                {
                    Surname = examinerViewModel.Surname,
                    Name = examinerViewModel.Name,
                    Patronymic = examinerViewModel.Patronymic,
                    Photo = uniqueFileName,
                    ExamPrice = examinerViewModel.ExamPrice
                };

                _context.Add(examiner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(examinerViewModel);
        }

        // загрузка файла и сохранение его с уникальным именем
        private string UploadedFile(ExaminerViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(webHostEnvironment.WebRootPath, "img", "persons", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Examiners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners.FindAsync(id);
            if (examiner == null)
            {
                return NotFound();
            }

            ExaminerViewModel viewModel = new ExaminerViewModel
            {
                Id = examiner.Id,
                Surname = examiner.Surname,
                Name = examiner.Name,
                Patronymic = examiner.Patronymic,
                ExamPrice = examiner.ExamPrice,
                PhotoPath = examiner.Photo
            };
            return View(viewModel);
        }

        // POST: Examiners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamPrice,Id,Surname,Name,Patronymic,Photo,PhotoPath")] ExaminerViewModel examinerViewModel)
        {
            if (id != examinerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Examiner examiner = new Examiner
                {
                    Id = examinerViewModel.Id,
                    Surname = examinerViewModel.Surname,
                    Name = examinerViewModel.Name,
                    Patronymic = examinerViewModel.Patronymic,
                    ExamPrice = examinerViewModel.ExamPrice,
                    Photo = examinerViewModel.PhotoPath
                };

                if (examinerViewModel.Photo != null)
                {
                    var fileName = UploadedFile(examinerViewModel);
                    examiner.Photo = fileName;
                }

                _context.Update(examiner);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(examinerViewModel);
        }

        // GET: Examiners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examiner == null)
            {
                return NotFound();
            }

            return View(examiner);
        }

        // POST: Examiners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examiner = await _context.Examiners.FindAsync(id);
            _context.Examiners.Remove(examiner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminerExists(int id)
        {
            return _context.Examiners.Any(e => e.Id == id);
        }
    }
}
