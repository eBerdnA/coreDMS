using CoreDMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.DBCreation
{
    public class DbCreationContext : DbContext
    {
        public virtual DbSet<LogTable> LogTable { get; set; }
        public DbCreationContext(DbContextOptions<DbCreationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogTable>(entity =>
            {
                entity.HasIndex(e => e.ScriptName)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.ScriptName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.ScriptOrder).HasColumnType("INT INTEGER");
            });
        }
    }
}
