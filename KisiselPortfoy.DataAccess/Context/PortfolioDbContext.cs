using Microsoft.EntityFrameworkCore;
using KisiselPortfoy.Core.Entities;

namespace KisiselPortfoy.DataAccess.Context
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<HomePageInfo> HomePageInfos { get; set; }
        public DbSet<ProfileInfo> ProfileInfos { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
