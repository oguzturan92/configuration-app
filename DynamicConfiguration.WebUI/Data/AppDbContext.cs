using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicConfiguration.WebUI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<ConfigurationItem> ConfigurationItems => Set<ConfigurationItem>();
    }
}