using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class CoursesDbContext : IdentityDbContext<AppUser>
    {
        public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        // DbSet properties for your entities
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseLessons> CourseLessons { get; set; }
    }
}
