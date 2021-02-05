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
    public class EnrolleesController : Controller
    {
        private readonly EntryExamsDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EnrolleesController(EntryExamsDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Enrollees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enrollees.ToListAsync());
        }

        // GET: Enrollees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollee = await _context.Enrollees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollee == null)
            {
                return NotFound();
            }

            return View(enrollee);
        }

        // GET: Enrollees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enrollees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrolleeViewModel enrolleeViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(enrolleeViewModel);

                Enrollee enrollee = new Enrollee
                {
                    Surname = enrolleeViewModel.Surname,
                    Name = enrolleeViewModel.Name,
                    Patronymic = enrolleeViewModel.Patronymic,
                    Address = enrolleeViewModel.Address,
                    BirthYear = enrolleeViewModel.BirthYear,
                    Passport = enrolleeViewModel.Passport,
                    Photo = uniqueFileName
                };

                _context.Add(enrollee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enrolleeViewModel);
        }

        // загрузка файла и сохранение его с уникальным именем
        private string UploadedFile(EnrolleeViewModel model)
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

        // GET: Enrollees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollee = await _context.Enrollees.FindAsync(id);
            if (enrollee == null)
            {
                return NotFound();
            }

            EnrolleeViewModel viewModel = new EnrolleeViewModel
            {
                Id = enrollee.Id,
                Surname = enrollee.Surname,
                Name = enrollee.Name,
                Patronymic = enrollee.Patronymic,
                Address = enrollee.Address,
                BirthYear = enrollee.BirthYear,
                Passport = enrollee.Passport,
                PhotoPath = enrollee.Photo
            };
            return View(viewModel);
        }

        // POST: Enrollees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,BirthYear,Passport,Id,Surname,Name,Patronymic,Photo,PhotoPath")] EnrolleeViewModel enrolleeViewModel)
        {
            if (id != enrolleeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                Enrollee enrollee = new Enrollee
                {
                    Id = enrolleeViewModel.Id,
                    Surname = enrolleeViewModel.Surname,
                    Name = enrolleeViewModel.Name,
                    Patronymic = enrolleeViewModel.Patronymic,
                    Address = enrolleeViewModel.Address,
                    BirthYear = enrolleeViewModel.BirthYear,
                    Passport = enrolleeViewModel.Passport,
                    Photo = enrolleeViewModel.PhotoPath
                };

                if (enrolleeViewModel.Photo != null)
                {
                    var fileName = UploadedFile(enrolleeViewModel);
                    enrollee.Photo = fileName;
                }

                _context.Update(enrollee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(enrolleeViewModel);
        }

        // GET: Enrollees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollee = await _context.Enrollees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollee == null)
            {
                return NotFound();
            }

            return View(enrollee);
        }

        // POST: Enrollees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollee = await _context.Enrollees.FindAsync(id);
            _context.Enrollees.Remove(enrollee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrolleeExists(int id)
        {
            return _context.Enrollees.Any(e => e.Id == id);
        }
    }
}
