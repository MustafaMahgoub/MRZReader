using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MRZReader.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Dal
{

    public class MrzReaderDbContext : IdentityDbContext
    {
        public MrzReaderDbContext(DbContextOptions<MrzReaderDbContext> options): base(options)
        {
            
        }
        public DbSet<MRZReader.Core.Document> Document { get; set; }
        
        //protected override void OnModelCreating(ModelBuilder builder)
        ////protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Seed();
        //}
    }



    
}
