using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using PensionerDetail.Entities;
//using System.Data.Entity;

namespace PensionerDetail.DbContexts
{
    public class PensionerDetailDbContext : DbContext
    {
        
        public PensionerDetailDbContext(DbContextOptions<PensionerDetailDbContext> options) : base(options) { }
        public DbSet<PensionerDetails> pensioner { get; set; }
    }
}
