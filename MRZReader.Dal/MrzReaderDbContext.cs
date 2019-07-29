using Microsoft.EntityFrameworkCore;
using MRZReader.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Dal
{

    public class MrzReaderDbContext : DbContext
    {
        // Testing the ExceptionAndTesting Branch
        public MrzReaderDbContext(DbContextOptions<MrzReaderDbContext> options): base(options)
        {
            
        }
        public DbSet<MRZReader.Core.Document> Document { get; set; }
    }



    
}
