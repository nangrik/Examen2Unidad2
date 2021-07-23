using Examen2Unidad2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Examen2Unidad2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TODO>tODOs { get; set; }
        public DbSet<DOING>dOINGs { get; set; }
        public DbSet<DONE> dONEs { get; set; }
    }
}
