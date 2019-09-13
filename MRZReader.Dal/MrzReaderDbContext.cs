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
        public DbSet<Document> Document { get; set; }
        public DbSet<User> User { get; set; }
    }
}
