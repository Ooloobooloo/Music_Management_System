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
    public class SongController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IMp3Service _mp3Service;
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();

        public SongController(AppDbContext context, IPhotoService photoService, IMp3Service mp3Service)
        {
            _context = context;
            _photoService = photoService;
            _mp3Service = mp3Service;
        }

        // GET: Song
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Songs.Include(s => s.Composer).Include(s => s.Singer);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Song/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Composer)
                .Include(s => s.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Song/Create
        public IActionResult Create()
        {
            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name");
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name");
            return View();
        }

        // POST: Song/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Lyrics,ThumbnailFile,MP3File,ReleaseDate,SingerId,ComposerId,Status")] Song song)
        {
            if (ModelState.IsValid)
            {
                // Sanitize Lyrics
                song.Lyrics = Sanitizer.Sanitize(song.Lyrics);

                // Handle thumbnail upload
                if (song.ThumbnailFile != null && song.ThumbnailFile.Length > 0)
                {
                    try
                    {
                        var photoResult = await _photoService.AddPhotoAsync(song.ThumbnailFile);
                        song.ThumbnailUrl = photoResult.SecureUrl?.ToString() ?? photoResult.Url?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ThumbnailFile", $"Thumbnail upload failed: {ex.Message}");
                        ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
                        ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
                        return View(song);
                    }
                }

                // Handle MP3 upload
                if (song.MP3File != null && song.MP3File.Length > 0)
                {
                    try
                    {
                        var audioResult = await _mp3Service.AddAudioAsync(song.MP3File);
                        song.MP3Url = audioResult.SecureUrl?.ToString() ?? audioResult.Url?.ToString() ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("MP3File", $"Audio upload failed: {ex.Message}");
                        ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
                        ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
                        return View(song);
                    }
                }

                song.CreatedAt = DateTime.Now;
                song.UpdatedAt = DateTime.Now;

                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
            return View(song);
        }

        // GET: Song/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
            return View(song);
        }

        // POST: Song/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Lyrics,ThumbnailUrl,MP3Url,ThumbnailFile,MP3File,ReleaseDate,CreatedAt,UpdatedAt,SingerId,ComposerId,Status")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }
            foreach (var kvp in ModelState)
            foreach (var err in kvp.Value.Errors)
                Console.WriteLine($"❌ {kvp.Key}: {err.ErrorMessage}");
            if (ModelState.IsValid)
            {
                try
                {
                    // Sanitize Lyrics
                    song.Lyrics = Sanitizer.Sanitize(song.Lyrics);

                    // Handle thumbnail upload
                    if (song.ThumbnailFile != null && song.ThumbnailFile.Length > 0)
                    {
                        try
                        {
                            var photoResult = await _photoService.AddPhotoAsync(song.ThumbnailFile);
                            song.ThumbnailUrl = photoResult.SecureUrl?.ToString() ?? photoResult.Url?.ToString() ?? string.Empty;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ThumbnailFile", $"Thumbnail upload failed: {ex.Message}");
                            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
                            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
                            return View(song);
                        }
                    }

                    // Handle MP3 upload
                    if (song.MP3File != null && song.MP3File.Length > 0)
                    {
                        try
                        {
                            var audioResult = await _mp3Service.AddAudioAsync(song.MP3File);
                            song.MP3Url = audioResult.SecureUrl?.ToString() ?? audioResult.Url?.ToString() ?? string.Empty;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("MP3File", $"Audio upload failed: {ex.Message}");
                            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
                            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
                            return View(song);
                        }
                    }

                    song.UpdatedAt = DateTime.Now;

                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
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
            ViewData["ComposerId"] = new SelectList(_context.Composers, "Id", "Name", song.ComposerId);
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", song.SingerId);
            return View(song);
        }

        // GET: Song/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Composer)
                .Include(s => s.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Song/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}