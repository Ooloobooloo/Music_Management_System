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
    public class SingerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();

        public SingerController(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        // GET: Singer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Singers.ToListAsync());
        }

        // GET: Singer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        // GET: Singer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Singer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Biography,ImageFile")] Singer singer)
        {
            if (ModelState.IsValid)
            {
                // Sanitize Biography
                singer.Biography = Sanitizer.Sanitize(singer.Biography);

                // Handle image upload
                if (singer.ImageFile != null && singer.ImageFile.Length > 0)
                {
                    try
                    {
                        var result = await _photoService.AddPhotoAsync(singer.ImageFile);
                        singer.ImageUrl = result.SecureUrl?.ToString() ?? result.Url?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                        return View(singer);
                    }
                }

                _context.Add(singer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(singer);
        }

        // GET: Singer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers.FindAsync(id);
            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);
        }

        // POST: Singer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Biography,ImageUrl,ImageFile")] Singer singer)
        {
            if (id != singer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Sanitize Biography
                    singer.Biography = Sanitizer.Sanitize(singer.Biography);

                    // Handle image upload
                    if (singer.ImageFile != null && singer.ImageFile.Length > 0)
                    {
                        try
                        {
                            var result = await _photoService.AddPhotoAsync(singer.ImageFile);
                            singer.ImageUrl = result.SecureUrl?.ToString() ?? result.Url?.ToString() ?? string.Empty;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                            return View(singer);
                        }
                    }

                    _context.Update(singer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SingerExists(singer.Id))
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
            return View(singer);
        }

        // GET: Singer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        // POST: Singer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var singer = await _context.Singers.FindAsync(id);
            if (singer != null)
            {
                _context.Singers.Remove(singer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SingerExists(int id)
        {
            return _context.Singers.Any(e => e.Id == id);
        }
    }
}