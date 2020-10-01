using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using FoodStyles.Data;
using FoodStyles.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<MenuItem> GetAll()
        {
            return _context.Foods;
        }

        public IEnumerable<MenuItem> ScrapeProcess(string startUrl)
        {
            try
            {
                this.ClearDb();
                Scrapper a = new Scrapper();
                var items = a.Parse();

                this.WriteToDb(items);
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
                throw;
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
    }
}