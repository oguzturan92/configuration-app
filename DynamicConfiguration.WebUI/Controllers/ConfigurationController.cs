using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;
using DynamicConfiguration.WebUI.Data;
using DynamicConfiguration.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicConfiguration.WebUI.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRedisService _redisService;

        public ConfigurationController(AppDbContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.ConfigurationItems.OrderBy(x => x.ApplicationName).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConfigurationItem item)
        {
            // MsSql'e eklenir
            _context.ConfigurationItems.Add(item);
            await _context.SaveChangesAsync();

            // Redis'e eklenir
            await _redisService.SaveAsync(item);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.ConfigurationItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConfigurationItem item)
        {
            var oldItem = await _context.ConfigurationItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
            if (oldItem == null) return NotFound();

            // MsSql'de güncellenir
            _context.ConfigurationItems.Update(item);
            await _context.SaveChangesAsync();

            // ApplicationName veya Name değişmişse, eski redis kaydı silinir
            if (oldItem.ApplicationName != item.ApplicationName || oldItem.Name != item.Name)
            {
                await _redisService.DeleteAsync(oldItem.ApplicationName, oldItem.Name);
            }

            // Redis'te güncellenir
            await _redisService.SaveAsync(item);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ConfigurationItems.FindAsync(id);
            if (item == null) return NotFound();

            // MSSQL'den sil
            _context.ConfigurationItems.Remove(item);
            await _context.SaveChangesAsync();

            // Redis'ten sil
            await _redisService.DeleteAsync(item.ApplicationName, item.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}