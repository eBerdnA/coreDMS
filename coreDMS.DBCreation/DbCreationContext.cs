using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace coreDMS.DBCreation
{
    public class DbCreationContext : DbContext
    {

        public DbCreationContext(DbContextOptions<DbCreationContext> options)
            : base(options)
        {
        }
    }
}
