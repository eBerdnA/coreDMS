using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.DBCreation
{
    public class DbCreationContext : DbContext
    {

        public DbCreationContext(DbContextOptions<DbCreationContext> options)
            : base(options)
        {
        }
    }
}
