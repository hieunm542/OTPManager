using JwtAuthDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Infrastructure
{
    public class DBContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<SMS> SMSs { get; set; }
    }
}
