using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreDMS.Model
{
    public partial class DMSContext : DbContext
    {
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<DocumentFiles> DocumentFiles { get; set; }
        public virtual DbSet<DocumentFileFile> DocumentFileFiles { get; set; }
        public virtual DbSet<FileStates> FileStates { get; set; }
        public virtual DbSet<FileTag> FileTag { get; set; }
        public virtual DbSet<LogTable> LogTable { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Upload> Upload { get; set; }
        public virtual DbSet<UploadError> UploadError { get; set; }

        public DMSContext(DbContextOptions<DMSContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Files>(entity =>
            {
                entity.HasIndex(e => e.Hash)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("VARCHAR(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("document_date")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasColumnName("hash")
                    .HasColumnType("VARCHAR(32)");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("VARCHAR(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("VARCHAR(255)")
                    .HasDefaultValueSql("`filename`");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");
            });

            modelBuilder.Entity<DocumentFiles>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("VARCHAR(255)")
                    .HasDefaultValueSql("`filename`");
            });

            modelBuilder.Entity<FileStates>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");
            });

            modelBuilder.Entity<DocumentFileFile>()
            .ToTable("DocumentFileFile");
            modelBuilder.Entity<DocumentFileFile>(entity => {
                #region properties
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");
                #endregion

                #region relationships
                entity.HasOne(d => d.File)
                    .WithMany(p => p.DocumentFileFile)
                    .HasForeignKey(d => d.FileId);

                entity.HasOne(d => d.DocumentFile)
                    .WithMany(p => p.DocumentFileFiles)
                    .HasForeignKey(d => d.DocumentFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                #endregion
            });

            modelBuilder.Entity<FileTag>(entity =>
            {
                #region properties
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.FileId).HasColumnType("VARCHAR(36)");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");
                #endregion

                #region relationships
                entity.HasOne(d => d.File)
                    .WithMany(p => p.FileTag)
                    .HasForeignKey(d => d.FileId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.FileTag)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                #endregion
            });

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

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("updatedAt")
                    .HasColumnType("DATETIME");
            });

            modelBuilder.Entity<Upload>()
            .ToTable("Uploads");

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");
            });

            modelBuilder.Entity<UploadError>()
            .ToTable("UploadErrors");

            modelBuilder.Entity<UploadError>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();                

                entity.Property(e => e.Error)
                    .HasColumnName("Error")
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("createdAt")
                    .HasColumnType("DATETIME");

                entity.HasOne(d => d.Upload)
                    .WithMany(p => p.UploadError)
                    .HasForeignKey(d => d.UploadId);
            });
        }
    }
}
