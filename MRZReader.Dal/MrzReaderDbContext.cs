using Microsoft.EntityFrameworkCore;
using MRZReader.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Dal
{

    public class MrzReaderDbContext : DbContext
    {
        public MrzReaderDbContext(DbContextOptions<MrzReaderDbContext> options): base(options)
        {
            
        }
        public DbSet<TestDocument> Documents { get; set; }
    }

    
}
