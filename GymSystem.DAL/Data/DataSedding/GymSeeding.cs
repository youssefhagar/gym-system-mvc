using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSedding
{
    public static class GymSeeding
    {

        public static async Task Seed(GymDbContext context,string filePath, ILogger logger, CancellationToken ct =default)
        {
            try
            {
                var plans  = LoadJsonData<Plan>(filePath, "plans.json");
                if(plans.Any())
                {                     
                    context.Plans.AddRange(plans);

                }
                if (context.ChangeTracker.HasChanges())
                    await context.SaveChangesAsync(ct);
                else
                    logger.LogInformation("No changes detected in the context. Seeding skipped.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
            
                
        }

        private static List<T> LoadJsonData<T>(string folderPath,string fileName) where T : class
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{fileName}' not found in folder '{folderPath}'.");
            }
            var jsonData = File.ReadAllText(filePath);
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(jsonData, option) ?? new List<T>();
        }

    }
}
