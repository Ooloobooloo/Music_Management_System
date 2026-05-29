using Microsoft.EntityFrameworkCore;
using Music_Management_System.Models;

namespace Music_Management_System.Data;

public static class DbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Đảm bảo Database đã được tạo thông qua Migrations
            context.Database.Migrate();

            // 1. Thực hiện Reset dữ liệu
            ClearData(context);

            // 2. Thực hiện Seeding dữ liệu mới
            SeedData(context);
        }
    }

    private static void ClearData(AppDbContext context)
    {
        // Cách 1: Sử dụng EF Core (An toàn cho mọi Database)
        // Lưu ý: Nếu có khóa ngoại (FK), phải xóa theo thứ tự bảng con trước, bảng cha sau.
        if (context.Composers.Any())
        {
            context.Composers.RemoveRange(context.Composers);
            context.SaveChanges();
        }


    }

    private static void SeedData(AppDbContext context)
    {
        private static void SeedData(AppDbContext context)
{
    if (!context.Composers.Any())
    {
        context.Composers.AddRange(new List<Composer>
        {
            new Composer { Name = "Big Chungus", Biography = " Born in 1926" },
            new Composer { Name = "Lil'Chungus", Biography = "Born in 1926" },
            new Composer { Name = "Medium-size Chungus", Biography = "Born in 1926" },
            new Composer { Name = "Slow Chungus", Biography = "Born in 1926" },
            new Composer { Name = "Fast Chungus", Biography = "Born in 1926" }
        });
        context.SaveChanges();
    }

    if (!context.Singers.Any())
    {
        context.Singers.AddRange(new List<Singer>
        {
            new Singer { Name = "Big Bingus", Biography = " Born in 1926" },
            new Singer { Name = "Lil' Bingus", Biography = "Born in 1926" },
            new Singer { Name = "Medium-size Bingus", Biography = "Born in 1926" },
            new Singer { Name = "Fast Bingus", Biography = "Born in 1926" },
            new Singer { Name = "Slow Bingus", Biography = "Born in 1926" }
        });
        context.SaveChanges();
    }

    if (!context.Songs.Any())
    {
        context.Songs.AddRange(new List<Song>
        {
            new Song { Title = "Z", Lyrics = " Funk", },
            new Song { Title = "ZZZZ", Lyrics = "Born in 1926" },
            new Song { Title = "ZZZZZZZ", Lyrics = "Born in 1926" },
            new Song { Title = "ZZZZZZZ", Lyrics = "Born in 1926" },
            new Song { Title = "ZZZZZZZZ", Lyrics = "Born in 1926" }
        });
        context.SaveChanges();
    }
}
        }
    }
