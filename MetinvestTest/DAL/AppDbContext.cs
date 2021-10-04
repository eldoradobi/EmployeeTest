using MetinvestTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MetinvestTest.DAL
{
    public class AppDbContext:DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}