using System;
using System.Collections.Generic;
using System.Text;
using Capstone2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Capstone2.Models.Cons> Cons { get; set; }
        public DbSet<Capstone2.Models.Pros> Pros { get; set; }

    }
}
