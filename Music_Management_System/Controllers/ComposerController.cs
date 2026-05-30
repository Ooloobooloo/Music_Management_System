using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Management_System.Data;
using Music_Management_System.Models;
using Music_Management_System.Interfaces;

namespace Music_Management_System.Controllers
{
    public class ComposerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();

        public ComposerController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        // GET: Composer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Composers.ToListAsync());
        }

        // GET: Composer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composer = await _context.Composers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composer == null)
            {
                return NotFound();
            }

            return View(composer);
        }

        // GET: Composer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Composer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Biography,ImageFile")] Composer composer)
        {
            if (ModelState.IsValid)
            {
                // Sanitize Biography
                composer.Biography = Sanitizer.Sanitize(composer.Biography);

                // Handle image upload
                if (composer.ImageFile != null && composer.ImageFile.Length > 0)
                {
                    try
                    {
                        var result = await _photoService.AddPhotoAsync(composer.ImageFile);
                        composer.ImageUrl = result.SecureUrl?.ToString() ?? result.Url?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                        return View(composer);
                    }
                }

                _context.Add(composer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(composer);
        }

        // GET: Composer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composer = await _context.Composers.FindAsync(id);
            if (composer == null)
            {
                return NotFound();
            }
            return View(composer);
        }

        // POST: Composer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Biography,ImageUrl,ImageFile")] Composer composer)
        {
            if (id != composer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Sanitize Biography
                    composer.Biography = Sanitizer.Sanitize(composer.Biography);

                    // Handle image upload
                    if (composer.ImageFile != null && composer.ImageFile.Length > 0)
                    {
                        try
                        {
                            var result = await _photoService.AddPhotoAsync(composer.ImageFile);
                            composer.ImageUrl = result.SecureUrl?.ToString() ?? result.Url?.ToString() ?? string.Empty;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                            return View(composer);
                        }
                    }

                    _context.Update(composer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComposerExists(composer.Id))
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
            return View(composer);
        }

        // GET: Composer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composer = await _context.Composers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composer == null)
            {
                return NotFound();
            }

            return View(composer);
        }

        // POST: Composer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var composer = await _context.Composers.FindAsync(id);
            if (composer != null)
            {
                _context.Composers.Remove(composer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComposerExists(int id)
        {
            return _context.Composers.Any(e => e.Id == id);
        }
    }
}