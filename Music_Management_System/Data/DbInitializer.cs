using Microsoft.EntityFrameworkCore;
using Music_Management_System.Models;
using Music_Management_System.Data;

namespace Music_Management_System.Data;

public static class DbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            context.Database.Migrate();
            Console.WriteLine("✅ Database migrated successfully.");

            // ONLY seed if there are no songs yet
            if (!context.Songs.Any())
            {
                Console.WriteLine("🌱 No data found. Starting seeding process...");

                ClearData(context);           // Safe because we checked Songs.Any()
                SeedData(context);

                Console.WriteLine("🎉 Sample data seeded successfully!");
            }
            else
            {
                Console.WriteLine($"ℹ️  {context.Songs.Count()} songs already exist in the database. Skipping seeding.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Seeding error: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
        }
    }

    private static void ClearData(AppDbContext context)
    {
        context.Songs.RemoveRange(context.Songs);
        context.Singers.RemoveRange(context.Singers);
        context.Composers.RemoveRange(context.Composers);
        context.SaveChanges();
        Console.WriteLine("   Old data cleared.");
    }

    private static void SeedData(AppDbContext context)
    {
        // === Seed Composers ===
        context.Composers.AddRange(new List<Composer>
        {
            new Composer { Name = "Lil' Chungus" },
            new Composer { Name = "Big Chungus" },
            new Composer { Name = "Alan Walker" },
            new Composer { Name = "Hans Zimmer" }
        });
        context.SaveChanges();
        Console.WriteLine($"   → Seeded {context.Composers.Count()} composers");

        // === Seed Singers ===
        context.Singers.AddRange(new List<Singer>
        {
            new Singer { Name = "Big Chungus" },
            new Singer { Name = "Lil' Chungus" },
            new Singer { Name = "Adele" },
            new Singer { Name = "The Weeknd" }
        });
        context.SaveChanges();
        Console.WriteLine($"   → Seeded {context.Singers.Count()} singers");

        // === Seed Songs ===
        var composerIds = context.Composers.Select(c => c.Id).ToList();
        var singerIds = context.Singers.Select(s => s.Id).ToList();
        var random = new Random();

        var titles = new[] { "Chungus Anthem", "Meat Excavation", "Pixel Dreams", "Quantum Chungus", "Neon Nights", "Midnight Meme" };

        var songs = new List<Song>();
        for (int i = 1; i <= 50; i++)
        {
            songs.Add(new Song
            {
                Title = titles[(i - 1) % titles.Length] + $" #{i}",
                Lyrics = "This is sample lyrics for testing the music management system.\n\n[Chorus]\nChungus never dies!",
                ThumbnailUrl = $"https://picsum.photos/id/{100 + (i % 50)}/600/400",
                MP3Url = i % 5 == 0 ? "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3" : null,
                ReleaseDate = DateTime.Now.AddDays(-random.Next(0, 200)),
                SingerId = singerIds[random.Next(singerIds.Count)],
                ComposerId = composerIds[random.Next(composerIds.Count)],
                Status = 1,                    // 1 = Active
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }

        context.Songs.AddRange(songs);
        context.SaveChanges();
        Console.WriteLine($"   → Seeded {songs.Count} songs");
    }
}