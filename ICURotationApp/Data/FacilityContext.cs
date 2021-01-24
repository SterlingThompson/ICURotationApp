using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ICURotationApp.Models
{
    public partial class FacilityContext : DbContext
    {
        public FacilityContext()
        {
        }

        public FacilityContext(DbContextOptions<FacilityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FacilityList> FacilityList { get; set; }

        public virtual DbSet<Denials> Denials { get; set; }

        public virtual DbSet<Acceptance> Acceptance { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FacilityContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacilityList>().Property(e => e.NumberOfSkips)
                .HasColumnName("NumberOfSkips");

            modelBuilder.Entity<FacilityList>(entity =>
            {
                entity.HasKey(e => e.FacilityId)
                    .HasName("PK__Facility__5FB08A741FB4236F");

                entity.Property(e => e.FacilityId).ValueGeneratedNever()
                .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.NextInRotation)
                .IsRequired();

               
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}