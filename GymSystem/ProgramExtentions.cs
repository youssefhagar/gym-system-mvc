using GymSystem.DAL.Data.DataSedding;
using GymSystem.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtentions
    {

        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingmigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            if(pendingmigrations.Any())
            {
                dbcontext.Database.Migrate();
            }
            string seedfolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "files");
            await GymSeeding.Seed(dbcontext, seedfolder, logger);

        } 


    }
}
