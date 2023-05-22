using Core.Entities;
using Core.Entities.Order;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Product_Category> Product_Categories { get; set; }


        //Order items
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product_Category>().HasKey(pc => new
            {
                pc.ProductId,
                pc.CategoryId
            });

            modelBuilder.Entity<Product_Category>().HasOne(m => m.Product).WithMany(am => am.Product_Categories).HasForeignKey(m => m.ProductId);
            modelBuilder.Entity<Product_Category>().HasOne(m => m.Category).WithMany(am => am.Product_Categories).HasForeignKey(m => m.CategoryId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
