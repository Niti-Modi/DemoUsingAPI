using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Data
{
    public class BookStoreContext : DbContext
    {

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }

        public DbSet<Books> Books { get; set; }
        public DbSet<EBooks> EBooksTable { get; set; }

        public DbSet<Customers> CustomersTable { get; set; }

        public DbSet<Orders> OrdersTable { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Orders>(build =>
            {
                build.HasKey(_ => _.OrderId);
            });

        }

    }

}
