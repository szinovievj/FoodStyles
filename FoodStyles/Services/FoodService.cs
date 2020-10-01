using System;
using System.Collections.Generic;
using FoodStyles.Data;
using FoodStyles.Utils;
using Microsoft.Extensions.Logging;

namespace FoodStyles.Services
{
    public class FoodService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FoodService> _logger;

        public FoodService(ApplicationDbContext context, ILogger<FoodService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<MenuItem> ScrapeProcess(string startUrl)
        {
            try
            {
                ClearDb();
                
                Scrapper scrapper = new Scrapper(startUrl);
                var items = scrapper.Parse();

                WriteToDb(items);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return GetAll();
        }
        
        
        private void ClearDb()
        {
            try
            {
                _logger.LogInformation("Clear database...");
                _context.Foods.RemoveRange(_context.Foods); ;
                _context.SaveChanges();
                _logger.LogInformation("Database was cleared");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
           
        }

        private void WriteToDb(IEnumerable<MenuItem> items)
        {
            _logger.LogInformation("Writing data to database..");
            foreach (var item in items)
            {
                _context.Foods.Add(item);
            }

            _context.SaveChanges();
            
            _logger.LogInformation("Data was saved successfully");
        }
        
        private IEnumerable<MenuItem> GetAll()
        {
            return _context.Foods;
        }
    }
}