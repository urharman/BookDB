using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookDB.Models;

namespace BookDB.DAL
{
    public class mySQLdbContext : DbContext
    {
        public mySQLdbContext(DbContextOptions<mySQLdbContext> options) : base(options)
        {
            //Constructor
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().ToTable("bookdb");
            builder.Entity<Book>().Property(t => t.ID_Book).HasColumnName("ID_Book");
            builder.Entity<Book>().Property(t => t.Name_Book).HasColumnName("Name_Book");
            builder.Entity<Book>().Property(t => t.Name_Author).HasColumnName("Name_Author");
            builder.Entity<Book>().Property(t => t.Price_Book).HasColumnName("Price_Book");
        }

        public virtual DbSet<Book> Books { get; set; }
    }

}
